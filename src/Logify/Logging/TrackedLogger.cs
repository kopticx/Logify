using Microsoft.Extensions.Logging;

namespace Logify.Logging;

public class TrackedLogger(ILoggerFactory loggerFactory) : ILogger
{
  private readonly ILogger _logger = loggerFactory.CreateLogger(Environment.GetEnvironmentVariable("loggerName"));
  public static string ExecutionName { get; set; } = Guid.NewGuid().ToString();

  public IDisposable BeginScope<TState>(TState state)
  {
    return _logger.BeginScope(state);
  }

  public bool IsEnabled(LogLevel logLevel)
  {
    return _logger.IsEnabled(logLevel);
  }

  public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
    Func<TState, Exception, string> formatter)
  {
    var message = formatter(state, exception);

    _logger.Log(logLevel, eventId, message, exception, (s, e) => s);
  }
}