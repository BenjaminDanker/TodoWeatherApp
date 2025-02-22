namespace TodoWeatherApp

module SecretsLoader =
    open System.IO
    open Newtonsoft.Json.Linq

    let loadUnsplashKey () =
        // Ensure secret.json is copied to the output directory.
        let secretJson = File.ReadAllText("secrets.json")
        let jObj = JObject.Parse(secretJson)
        // Access the Unsplash key dynamically:
        jObj.["Unsplash"].["AccessKey"].ToString()
