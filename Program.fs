module TodoWeatherApp.Program

open Giraffe
open Giraffe.EndpointRouting
open Handlers
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Hosting
open TodoHandlers

let endpoints = [
    GET [
        route "/weatherforecast" get
        route "/todos" getAllTodos
        route "/weather" getWeatherHandler
    ]
    POST [
        route "/todos" addTodo
    ]
    PUT [
        routef "/todos/%i" updateTodo
    ]
    DELETE [
        routef "/todos/%i" deleteTodo
    ]
]

[<EntryPoint>]
let main args =
    let builder = WebApplication.CreateBuilder(args)

    builder.Services.AddGiraffe() |> ignore

    let app = builder.Build()

    app.UseHttpsRedirection() |> ignore

    app.UseRouting().UseGiraffe(endpoints) |> ignore

    app.Run()

    0
