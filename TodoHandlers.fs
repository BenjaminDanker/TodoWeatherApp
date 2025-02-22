namespace TodoWeatherApp

module TodoHandlers =

    // Define TodoItem
    type TodoItem = {
        Id: int
        Title: string
        IsDone: bool
    }

    // A mutable store for demonstration. In production, use a database.
    let private todos = System.Collections.Concurrent.ConcurrentBag<TodoItem>()
    let mutable currentId = 1

    open System
    open System.Collections.Concurrent
    open Microsoft.AspNetCore.Http
    open Giraffe
    open FSharp.Control.Tasks.V2.ContextInsensitive // Needed for `task {}` support

    let getAllTodos : HttpHandler =
        fun next ctx ->
            let allTodos = todos |> Seq.toList
            json allTodos next ctx

    let addTodo : HttpHandler =
        fun next ctx ->
            task {
                let! newTodo = ctx.BindJsonAsync<TodoItem>()
                let todoWithId = { newTodo with Id = currentId }
                currentId <- currentId + 1
                todos.Add todoWithId
                return! json todoWithId next ctx
            }

    let updateTodo (id: int) : HttpHandler =
        fun next ctx ->
            task {
                let! updated = ctx.BindJsonAsync<TodoItem>()
                // In-memory update – typically you'd handle concurrency, etc.
                let existing = todos |> Seq.tryFind (fun t -> t.Id = id)
                match existing with
                | Some old ->
                    let mutable temp = old
                    todos.TryTake(&temp) |> ignore
                    todos.Add { updated with Id = id }
                    return! json updated next ctx
                | None -> return! RequestErrors.NOT_FOUND $"Todo with ID {id} not found" next ctx
            }

    let deleteTodo (id: int) : HttpHandler =
        fun next ctx ->
            task {
                let existing = todos |> Seq.tryFind (fun t -> t.Id = id)
                match existing with
                | Some old ->
                    let mutable temp = old
                    todos.TryTake(&temp) |> ignore
                    return! Successful.OK $"Todo with ID {id} deleted." next ctx
                | None ->
                    return! RequestErrors.NOT_FOUND $"Todo with ID {id} not found" next ctx
            }
