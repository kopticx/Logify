using Logify.Logging;
using System.Text.Json;
using Logify.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Logify;

public static class LogifyRegistration
{
  public static IServiceCollection AddLogifyService(this IServiceCollection services,
    JsonSerializerOptions jsonSerializerOptions = null, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
  {
    services.Add(new ServiceDescriptor(typeof(ILogger), typeof(TrackedLogger), serviceLifetime));

    if (jsonSerializerOptions is not null)
    {
      LogifyExtensions.Configure(jsonSerializerOptions);
    }

    return services;
  }
}