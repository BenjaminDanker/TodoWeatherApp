namespace TodoWeatherApp

module ImageHandlers =
    open Giraffe
    open Microsoft.Extensions.DependencyInjection

    let getImageHandler : HttpHandler =
        fun next ctx ->
            task {
                // Retrieve configuration from DI
                let config = ctx.RequestServices.GetService<AppConfig>()
                let unsplashKey = config.UnSplash.Key
                
                let! image = ImageService.getRandomImage unsplashKey |> Async.StartAsTask
                return! json image next ctx
            }
