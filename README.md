# TodoWeatherApp

**TodoWeatherApp** is an F# web application built using [Giraffe](https://github.com/giraffe-fsharp/Giraffe) that combines a simple Todo list API with weather forecast functionality. It also includes image handling and scheduled email notifications to showcase practical integration of multiple features into one app.

## Features

- **Todo Management:** Create, update, retrieve, and delete Todo items through RESTful endpoints.
- **Weather Forecast:** Retrieve weather forecast data using a dedicated endpoint.
- **Image Handling:** Serve image content through a designated endpoint.
- **Email Notifications:** Schedule daily email notifications using the built-in email service.

## Project Structure

The project is organized by feature to promote clarity and maintainability. Here’s an overview of the folder layout:

```
/src
  /Todo
    TodoHandlers.fs      // Handles API endpoints for todos
  /Weather
    WeatherForecast.fs   // Domain logic for weather forecasting
    WeatherForecastHandlers.fs // Endpoints for retrieving weather data
  /Images
    Images.fs            // Domain logic for image handling
    ImagesHandlers.fs    // Endpoints for serving images
  /Email
    Email.fs             // Email service for notifications
  /Models
    Models.fs            // Shared domain models
  Config.fs              // Configuration loader
  Program.fs             // Entry point for the application
```

## Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (recommended version: .NET 6 or later)
- F# development tools (e.g., Visual Studio Code with Ionide, or Visual Studio)

### Running the Application

1. **Clone the repository:**

   ```bash
   git clone https://github.com/BenjaminDanker/TodoWeatherApp.git
   cd TodoWeatherApp
   ```

2. **Build and run:**

   ```bash
   dotnet build
   dotnet run
   ```

3. **Access the application:**

   Open your browser and navigate to the default URL (typically `https://localhost:5001` or `http://localhost:5000`).

## API Endpoints

The following endpoints are available:

- **GET /weatherforecast** – Retrieves the weather forecast data.
- **GET /todos** – Retrieves all Todo items.
- **POST /todos** – Adds a new Todo item.
- **PUT /todos/{id}** – Updates an existing Todo item.
- **DELETE /todos/{id}** – Deletes a Todo item.
- **GET /image** – Serves image content.

## Configuration

The application loads its configuration via a custom configuration loader defined in `Config.fs`. Ensure you have your configuration settings properly set in the environment or configuration files as needed.


Example:
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "UnSplash": {
    "Key": "YOUR_UNSPLASH_KEY"
  },
  "MailGun": {
    "Server": "smtp.mailgun.org",
    "Port": 587,
    "Username": "postmaster@sandbox{MailGun_Numerals}.mailgun.org",
    "Password": "YOUR_MAILGUN_PASSWORD",
    "Recipient": "RECEIVING_EMAIL_ADDRESS"
  }
}
```

## Scheduled Email Service

The email service, implemented in `Email.fs`, is scheduled to send daily notifications. This demonstrates how to integrate background services with your web application.

## License

Distributed under the MIT License. See [LICENSE](LICENSE) for more information.
