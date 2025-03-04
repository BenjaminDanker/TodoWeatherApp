namespace TodoWeatherApp

module ImageService =
    open System.Net.Http

    [<Literal>]
    let sampleJson = """
    {
        "urls": { "regular": "https://images.unsplash.com/photo-xxx" },
        "alt_description": "A sample image"
    }"""
    type UnsplashResponse = FSharp.Data.JsonProvider<sampleJson>

    let httpClient = new HttpClient()
    
    let getRandomImage (accessKey: string) = async {
        httpClient.DefaultRequestHeaders.Clear()
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Client-ID {accessKey}")
        let! response = httpClient.GetStringAsync("https://api.unsplash.com/photos/random") |> Async.AwaitTask
        let data = UnsplashResponse.Parse(response)
        return {
            Url = data.Urls.Regular
            Description =
                if System.String.IsNullOrWhiteSpace(data.AltDescription) then
                    "An Unsplash image"
                else
                    data.AltDescription
        }

    }
