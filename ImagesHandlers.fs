namespace TodoWeatherApp

module ImageHandlers =
    open Microsoft.AspNetCore.Http
    open Giraffe
    open Microsoft.Extensions.Configuration

    let getImageHandler : HttpHandler =
        fun next ctx ->
            task {
                let unsplashKey = SecretsLoader.getUnsplashKey()
                printfn "Unsplash Key: %s" unsplashKey
                let! image = ImageService.getRandomImage unsplashKey |> Async.StartAsTask
                // Removed duplicate asynchronous call
                return! json image next ctx
            }
