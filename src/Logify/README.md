# Kopticx.Logify

[![NuGet](https://img.shields.io/nuget/v/Kopticx.Logify?style=flat-square)](https://www.nuget.org/packages/Kopticx.Logify)
[![.NET](https://img.shields.io/badge/.NET-8.0-blue?style=flat-square)](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

**Kopticx.Logify** is a lightweight logging library for C# that implements a custom `ILogger` with traceable naming, ideal for distributed systems, background workers, and microservices where identifying the log origin is essential.

---

## ✨ Features

- 🔧 Custom logger with dynamic naming
- 🧩 Compatible with `Microsoft.Extensions.Logging`
- 🪄 Simple integration with `Microsoft.Extensions.DependencyInjection`
- 📦 Published as a NuGet package
- ⚙️ Native AOT (Ahead-of-Time) compatible
- 🧪 Ideal for serverless apps, background jobs, and microservices

---

## 🚀 Installation

Install from NuGet:

```bash
dotnet add package Kopticx.Logify
```

---

## ⚙️ How it works

Logify lets you log objects and exceptions without manually serializing them.  
It provides convenient extension methods to cover the most common logging scenarios:

```csharp
logger.LogInformationCustom(new { data, files });
logger.LogInformationCustom("Informative message", new { data, files });
logger.LogErrorCustom(new { data, files }, exception);
logger.LogErrorCustom("Failed to save to DB", new { context = "Import process", id }, exception);
```

These `Custom` methods have overloads that allow you to include an optional message that is later attached to the final log object.

Here’s how a log entry looks when using `LogInformationCustom` or `LogErrorCustom` with a custom object:

```text
[Information] TestLogify: 
{
    "TrackerId": "490c6df3-eed4-490d-bfe5-c491e9fea278",
    "Message": "Informative message",
    "CustomObject": {
        "data": {},
        "files": []
    }
}
```

---

## 🛠️ Usage

### 1. Register the service

Add the logger to your DI container:

```csharp
builder.Services.AddLogify();
```

If you're using Native AOT, register with your custom `TypeInfoResolver` like this:

```csharp
var jsonSerializerOptions = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
    TypeInfoResolver = AppJsonSerializerContext.Default,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};

services.AddLogifyService(jsonSerializerOptions);
```

> By default, Logify registers as a `Singleton` to reuse the same `TrackerId` throughout the application's lifetime.  
> You can change the service lifetime to `Transient` or `Scoped` if you need a different `TrackerId` per HTTP request or instance.

---

### 2. Inject `ILogger`

Since Logify extends the native .NET logger, you can simply inject `ILogger`:

```csharp
public class Worker(ILogger logger)
{
    public Task Run()
    {
        logger.LogInformationCustom("Hello");
        return Task.CompletedTask;
    }
}
```

---

### 3. Full example in `Program.cs`

```csharp
var builder = Host.CreateApplicationBuilder(args);

// Register Logify
builder.Services.AddLogify();

// Register your actual logger provider (Console, AWS, OpenTelemetry, etc.)
// ...

// Sample hosted service
builder.Services.AddHostedService<Worker>();

var app = builder.Build();
app.Run();
```

---

### 4. Set the logger name

You can define the logger name using the environment variable:

```bash
export loggerName="MyCustomLogger"
```

- If the environment variable `loggerName` is defined, it will be used.
- If not, the application name (`System.AppDomain.CurrentDomain.FriendlyName`) will be used by default.

---

## 📂 Project Structure

- `TrackedLogger.cs`: Custom `ILogger` implementation with dynamic naming
- `LogifyExtensions.cs`: Extension methods like `LogInformationCustom`, `LogErrorCustom`, etc.
- `LogifyRegistration.cs`: Dependency injection registration logic

---

## 🔐 Native AOT Support

Logify is fully compatible with Native AOT for optimized, self-contained executables:

```xml
<IsAotCompatible>true</IsAotCompatible>
```

---

## 📦 NuGet Package

📦 [nuget.org/packages/Kopticx.Logify](https://www.nuget.org/packages/Kopticx.Logify)

---

## 🧪 Why use Logify?

- Log structured objects without manually formatting them
- Add contextual metadata automatically (e.g., `TrackerId`)
- Great for debugging, monitoring, and traceability in production
- Simplifies logging in .NET applications

---

## 📝 License

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](./LICENSE)