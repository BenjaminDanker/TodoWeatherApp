namespace TodoWeatherApp

module ImageHandlers =
    open Microsoft.AspNetCore.Http
    open Giraffe
    open Microsoft.Extensions.DependencyInjection
    open TodoWeatherApp.ConfigLoader  // Import the config module

    let getImageHandler : HttpHandler =
        fun next ctx ->
            task {
                // Retrieve configuration from DI
                let config = ctx.RequestServices.GetService<AppConfig>()
                let unsplashKey = config.UnSplash.Key

                printfn "Unsplash Key: %s" unsplashKey

                let! image = ImageService.getRandomImage unsplashKey |> Async.StartAsTask
                return! json image next ctx
            }
