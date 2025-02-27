module TodoWeatherApp.Program

open Giraffe
open Giraffe.EndpointRouting
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Hosting

let endpoints = [
    GET [
        route "/weatherforecast" Handlers.get
        route "/todos" TodoHandlers.getAllTodos
        route "/image" ImageHandlers.getImageHandler
    ]
    POST [
        route "/todos" TodoHandlers.addTodo
    ]
    PUT [
        routef "/todos/%i" TodoHandlers.updateTodo
    ]
    DELETE [
        routef "/todos/%i" TodoHandlers.deleteTodo
    ]
]

[<EntryPoint>]
let main args =
    let builder = WebApplication.CreateBuilder(args)

    builder.Services.AddGiraffe() |> ignore

    let app = builder.Build()

    app.UseHttpsRedirection() |> ignore

    app.UseRouting().UseGiraffe(endpoints) |> ignore

    EmailService.scheduleDailyEmail(app.Services)

    app.Run()

    0
