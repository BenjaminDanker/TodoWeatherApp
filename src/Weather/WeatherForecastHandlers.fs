module TodoWeatherApp.Handlers

open System
open Giraffe
open Microsoft.AspNetCore.Http


let summaries =
    [| "Freezing"
       "Bracing"
       "Chilly"
       "Cool"
       "Mild"
       "Warm"
       "Balmy"
       "Hot"
       "Sweltering"
       "Scorching" |]

let getWeatherHandler : HttpHandler =
    fun next ctx ->
        printfn "[INFO] Received request: GET /weatherforecast"

        task {
            let! weather = WeatherService.fetchWeather 40.0 -74.0 |> Async.StartAsTask
            return! json weather next ctx
        }
