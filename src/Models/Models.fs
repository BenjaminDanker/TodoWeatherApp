namespace TodoWeatherApp

type TodoItem = {
    Id: int
    Title: string
    IsDone: bool
}

type WeatherData = {
    Date: System.DateTime
    TemperatureC: float
    Description: string
}

type ImageData = {
    Url: string
    Description: string
}
