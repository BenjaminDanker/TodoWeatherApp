namespace TodoWeatherApp

module TodoHandlers =

    open Giraffe
    open FSharp.Control.Tasks.V2.ContextInsensitive
    open System.Collections.Concurrent

    // A mutable store for demonstration. In production, use a database.
    let private todos = new ConcurrentDictionary<int, TodoItem>()
    let mutable currentId = 1

    let getTodos () =
        todos.Values |> Seq.toList

    let getAllTodos : HttpHandler =
        fun next ctx ->
            printfn "[INFO] Received request: GET /todos"
            let allTodos = todos.Values |> Seq.toList
            json allTodos next ctx

    let addTodo : HttpHandler =
        fun next ctx ->
            printfn "[INFO] Received request: POST /todos"
            task {
                let! newTodo = ctx.BindJsonAsync<TodoItem>()
                printfn "[INFO] Received new todo: %A" newTodo
                let todoWithId = { newTodo with Id = currentId }
                currentId <- currentId + 1
                todos.TryAdd(todoWithId.Id, todoWithId) |> ignore
                printfn "[INFO] All todos: %A" (todos.Values |> Seq.toList)
                return! json todoWithId next ctx
            }

    let updateTodo (id: int) : HttpHandler =
        fun next ctx ->
            printfn $"[INFO] Received request: PUT /todos/{id}"
            task {
                let! updated = ctx.BindJsonAsync<TodoItem>()
                if todos.ContainsKey id then
                    // Replace the todo with the updated version (keeping the same id)
                    todos.[id] <- { updated with Id = id }
                    return! json (todos.[id]) next ctx
                else
                    return! RequestErrors.NOT_FOUND $"Todo with ID {id} not found" next ctx
            }

    let deleteTodo (id: int) : HttpHandler =
        fun next ctx ->
            printfn $"[INFO] Received request: DELETE /todos/{id}"
            task {
                match todos.TryRemove(id) with
                | true, _ ->
                    return! Successful.OK $"Todo with ID {id} deleted." next ctx
                | false, _ ->
                    return! RequestErrors.NOT_FOUND $"Todo with ID {id} not found" next ctx
            }
