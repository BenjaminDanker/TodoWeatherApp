namespace TodoWeatherApp

open Microsoft.Extensions.Configuration

type UnSplashConfig = {
    Key: string
}

type MailGunConfig = {
    Server: string
    Port: int
    Username: string
    Password: string
    Recipient: string
}

type AppConfig = {
    UnSplash: UnSplashConfig
    MailGun: MailGunConfig
}

module ConfigLoader =
    let loadConfiguration (configuration: IConfiguration) =
        let unsplashConfig = configuration.GetSection("UnSplash").Get<UnSplashConfig>()
        let mailGunConfig = configuration.GetSection("MailGun").Get<MailGunConfig>()
        { UnSplash = unsplashConfig; MailGun = mailGunConfig }