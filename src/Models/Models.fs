namespace TodoWeatherApp

type TodoItem = {
    Id: int
    Title: string
    IsDone: bool
}

type WeatherData = {
    Date: System.DateTime
    TemperatureC: float
}

type ImageData = {
    Url: string
    Description: string
}
