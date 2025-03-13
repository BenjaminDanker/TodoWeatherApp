module TodoWeatherApp.Program

open Giraffe
open Giraffe.EndpointRouting
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Hosting
open TodoWeatherApp.ConfigLoader
open Microsoft.Extensions.DependencyInjection

let endpoints = [
    GET [
        route "/weatherforecast" Handlers.getWeatherHandler
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

    // Load config, add services, etc.
    let config = loadConfiguration builder.Configuration
    builder.Services.AddSingleton(config) |> ignore
    builder.Services.AddGiraffe() |> ignore

    let app = builder.Build()

    // 1. Automatically serve index.html if users hit "/"
    app.UseDefaultFiles() |> ignore

    // 2. Serve static files from 'wwwroot'
    app.UseStaticFiles() |> ignore

    // 3. Other middlewares
    app.UseHttpsRedirection() |> ignore

    // 4. Giraffe routing (API endpoints, etc.)
    app.UseRouting().UseGiraffe(endpoints) |> ignore

    EmailService.scheduleDailyEmail(app.Services)

    app.Run()
    0
