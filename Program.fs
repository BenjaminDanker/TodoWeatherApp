module TodoWeatherApp.Program

open Giraffe
open Giraffe.EndpointRouting
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Hosting
open TodoWeatherApp.ConfigLoader
open Microsoft.Extensions.DependencyInjection

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

    // Load configuration from appsettings.json
    let config = loadConfiguration builder.Configuration

    // Register AppConfig as a singleton
    builder.Services.AddSingleton(config) |> ignore
    builder.Services.AddGiraffe() |> ignore

    let app = builder.Build()

    app.UseHttpsRedirection() |> ignore
    app.UseRouting().UseGiraffe(endpoints) |> ignore

    EmailService.scheduleDailyEmail(app.Services)

    app.Run()
    0
