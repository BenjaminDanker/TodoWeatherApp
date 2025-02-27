namespace TodoWeatherApp

module SecretsLoader =
    open System.IO
    open Newtonsoft.Json.Linq

    // Load and parse the JSON file once.
    let private secrets =
        let secretJson = File.ReadAllText("secrets.json")
        JObject.Parse(secretJson)

    /// Retrieve the Unsplash Access Key.
    let getUnsplashKey () =
        secrets.["Unsplash"].["AccessKey"].ToString()

    /// Retrieve the SMTP server for email.
    let getEmailSmtpServer () =
        secrets.["Email"].["SmtpServer"].ToString()

    /// Retrieve the SMTP port as an integer.
    let getEmailSmtpPort () =
        secrets.["Email"].["SmtpPort"].ToString() |> int

    /// Retrieve the Email user (sender).
    let getEmailUser () =
        secrets.["Email"].["User"].ToString()

    /// Retrieve the Email password.
    let getEmailPassword () =
        secrets.["Email"].["Password"].ToString()

    /// Retrieve the Email recipient.
    let getEmailRecipient () =
        secrets.["Email"].["Recipient"].ToString()
