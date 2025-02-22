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

let get =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        let data =
            [| 0..4 |]
            |> Array.map (fun index ->
                { Date = DateTime.Now.AddDays(float index)
                  TemperatureC = Random.Shared.Next(-20, 55)
                  Summary = summaries.[Random.Shared.Next(summaries.Length)] })

        json data next ctx
