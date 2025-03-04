namespace TodoWeatherApp

open FSharp.Data

module WeatherService =
    open System.Net.Http
    open FSharp.Data // or use native HttpClient with JSON parser

    // Example with FSharp.Data's JsonProvider
    // If you prefer to do it manually, you can use System.Text.Json or Newtonsoft.Json

    [<Literal>]
    let sampleJson = """ { "hourly": { "temperature_2m": [ 0.1 ] } } """
    type OpenMeteoResponse = JsonProvider<sampleJson>


    let httpClient = new HttpClient()

    let fetchWeather (lat: float) (lon: float) = async {
        // public url, doesn't need to be obscured
        let url = sprintf "https://api.open-meteo.com/v1/forecast?latitude=%f&longitude=%f&hourly=temperature_2m" lat lon
        let! response = httpClient.GetStringAsync url |> Async.AwaitTask

        // Give errors but work??
        let data = OpenMeteoResponse.Parse(response)
        let temp = data.Hourly.Temperature2m.[0]

        let now = System.DateTime.UtcNow
        return { Date = now; TemperatureC = float temp; Description = "N/A" }
    }
