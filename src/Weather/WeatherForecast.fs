namespace TodoWeatherApp

open FSharp.Data

module WeatherService =
    open System.Net.Http
    open FSharp.Data

    [<Literal>]
    let sampleJson = """
    {
      "hourly": {
        "time": ["2025-03-04T00:00", "2025-03-04T01:00"],
        "temperature_2m": [2.4, 2.5]
      }
    }"""

    type OpenMeteoResponse = JsonProvider<sampleJson>

    let httpClient = new HttpClient()

    let fetchWeather (lat: float) (lon: float) = async {
        let url = sprintf "https://api.open-meteo.com/v1/forecast?latitude=%f&longitude=%f&hourly=temperature_2m" lat lon
        try
            let! response = httpClient.GetStringAsync(url) |> Async.AwaitTask
            let data = OpenMeteoResponse.Parse(response)
            
            // Zip together the time and temperature arrays without parsing the time again
            let forecasts = 
                Array.zip data.Hourly.Time data.Hourly.Temperature2m
                |> Array.map (fun (time, temp) ->
                    { TodoWeatherApp.WeatherData.Date = time
                      TemperatureC = float temp})
            
            return forecasts
        with ex ->
            printfn "[ERROR] Weather fetch failed: %s" ex.Message
            return failwith "Failed to fetch weather"
    }
