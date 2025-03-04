namespace TodoWeatherApp

module EmailService =
    open MailKit.Net.Smtp
    open MimeKit
    open System
    open System.Threading
    open Microsoft.Extensions.DependencyInjection

    let sendEmail (config: MailGunConfig) (toAddress: string) (subject: string) (body: string) =
        let message = new MimeMessage()
        message.From.Add(MailboxAddress("TodoWeatherApp", config.Username))
        message.To.Add(MailboxAddress("", toAddress))
        message.Subject <- subject

        let bodyBuilder = new BodyBuilder(TextBody = body)
        message.Body <- bodyBuilder.ToMessageBody()

        use client = new SmtpClient()
        client.Connect(config.Server, config.Port, MailKit.Security.SecureSocketOptions.StartTls)
        client.Authenticate(config.Username, config.Password)
        client.Send(message) |> ignore
        client.Disconnect(true)

    let sendDailyEmail (services: IServiceProvider) =
        let config = services.GetService<AppConfig>() // Get AppConfig

        let todos = TodoHandlers.getTodos ()
        let weather = WeatherService.fetchWeather 37.6872 -97.3301 |> Async.RunSynchronously
        let image = ImageService.getRandomImage config.UnSplash.Key |> Async.RunSynchronously

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

        sendEmail config.MailGun config.MailGun.Recipient "Daily Updates" emailBody

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
