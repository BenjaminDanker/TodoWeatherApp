namespace TodoWeatherApp

module EmailService =
    open MailKit.Net.Smtp
    open MimeKit
    open System
    open System.Threading
    open Microsoft.Extensions.Configuration

    let sendEmail (smtpServer: string) (smtpPort: int) (user: string) (password: string) (toAddress: string) (subject: string) (body: string) =
        let message = new MimeMessage()
        message.From.Add(MailboxAddress("TodoWeatherApp", user))
        message.To.Add(MailboxAddress("", toAddress))
        message.Subject <- subject

        let bodyBuilder = new BodyBuilder(TextBody = body)
        message.Body <- bodyBuilder.ToMessageBody()

        use client = new SmtpClient()
        client.Connect(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls)
        client.Authenticate(user, password)
        client.Send(message) |> ignore
        client.Disconnect(true)

    /// This function gathers data and sends the daily email.
    let sendDailyEmail (services: IServiceProvider) =
        // Retrieve configuration from services
        let smtpServer = SecretsLoader.getEmailSmtpServer()
        let smtpPort = SecretsLoader.getEmailSmtpPort()
        let user = SecretsLoader.getEmailUser()
        let password = SecretsLoader.getEmailPassword()
        let recipient = SecretsLoader.getEmailRecipient()
        let unsplashKey = SecretsLoader.getUnsplashKey()


        // Debug prints
        printfn "SMTP Server: %A" smtpServer
        printfn "SMTP Port: %A" smtpPort
        printfn "User: %A" user
        printfn "Recipient: %A" recipient
        
        // Retrieve your daily data (todos, weather, image) here.
        // For example, assuming you have the following functions:
        let todos = TodoHandlers.getTodos ()
        let weather = WeatherService.fetchWeather 37.6872 -97.3301|> Async.RunSynchronously
        let image = ImageService.getRandomImage unsplashKey |> Async.RunSynchronously

        // Build email body text.
        let todosText =
            if List.isEmpty todos then
                "No todos for today."
            else
                todos 
                |> List.map (fun todo -> sprintf "- %s (Done: %b)" todo.Title todo.IsDone)
                |> String.concat "\n"
                
        let emailBody =
            sprintf "Good morning!\n\nHere are your daily updates:\n\nTodos:\n%s\n\nWeather:\nDate: %A\nTemperature: %d°C\n\nImage:\nURL: %s\nDescription: %s"
                todosText weather.Date (int weather.TemperatureC) image.Url image.Description

        // Send the email. For testing, use your own authorized recipient.
        sendEmail 
            smtpServer smtpPort
            user password
            recipient
            "Daily Updates"
            emailBody

    /// This function schedules the daily email.
    let scheduleDailyEmail (services: IServiceProvider) =
        let rec scheduleNextRun () =
            let now = DateTime.Now
            let target = now.Date.AddHours(6.0)  // 6:00 AM
            let mutable delay = target - now
            if delay < TimeSpan.Zero then
                delay <- delay.Add(TimeSpan.FromDays(1.0))
            delay

        let backgroundThread = new Thread(ThreadStart(fun () ->
            while true do
                let delay = scheduleNextRun ()
                Thread.Sleep(delay)
                sendDailyEmail services
        ))
        backgroundThread.IsBackground <- true
        backgroundThread.Start()