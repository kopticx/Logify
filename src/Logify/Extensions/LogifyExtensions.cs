using System.Text.Json;
using Logify.Logging;
using Microsoft.Extensions.Logging;

namespace Logify.Extensions;

public static class LogifyExtensions
{
  private static JsonSerializerOptions _jsonSerializerOptions = new();

  internal static void Configure(JsonSerializerOptions options)
  {
    _jsonSerializerOptions = options;
  }

  /// <summary>
  /// Logs an informational message using a custom object. 
  /// If the object is a string, it is logged as-is; otherwise, it is serialized to JSON.
  /// A tracker ID is automatically included in the log.
  /// </summary>
  /// <typeparam name="T">The type of the custom object to log.</typeparam>
  /// <param name="logger">The <see cref="ILogger"/> instance used for logging.</param>
  /// <param name="customObject">
  /// The object to log. If it is a <see cref="string"/>, it will be logged directly; 
  /// otherwise, it will be serialized to JSON before logging.
  /// </param>
  public static void LogInformationCustom<T>(this ILogger logger, T customObject)
  {
    var type = customObject.GetType();

    if (type == typeof(string))
    {
      var customMessage = GetStringObject(customObject.ToString());

      logger.Log(LogLevel.Information, customMessage);

      return;
    }

    var customMessageJson = GetObjectLogging(customObject);

    logger.Log(LogLevel.Information, customMessageJson);
  }

  /// <summary>
  /// Logs an informational message with an accompanying message and a custom object. 
  /// If the object is a string, it is logged as-is; otherwise, it is serialized to JSON.
  /// A tracker ID is automatically included in the log.
  /// </summary>
  /// <typeparam name="T">The type of the custom object to log.</typeparam>
  /// <param name="logger">The <see cref="ILogger"/> instance used for logging.</param>
  /// <param name="message">A custom message to include with the log entry.</param>
  /// <param name="customObject">
  /// The object to log. If it is a <see cref="string"/>, it will be logged directly;
  /// otherwise, it will be serialized to JSON before logging.
  /// </param>
  public static void LogInformationCustom<T>(this ILogger logger, string message, T customObject)
  {
    var customMessageJson = GetMessageObject(message, customObject);

    logger.Log(LogLevel.Information, customMessageJson);
  }

  /// <summary>
  /// Logs an error message with a custom object and an associated exception. 
  /// If the object is a string, it is logged as-is; otherwise, it is serialized to JSON.
  /// A tracker ID is automatically included in the log.
  /// </summary>
  /// <typeparam name="T">The type of the custom object to log.</typeparam>
  /// <param name="logger">The <see cref="ILogger"/> instance used for logging.</param>
  /// <param name="customObject">
  /// The object to log. If it is a <see cref="string"/>, it will be logged directly;
  /// otherwise, it will be serialized to JSON before logging.
  /// </param>
  /// <param name="exception">The <see cref="Exception"/> to be logged alongside the custom object.</param>
  public static void LogErrorCustom<T>(this ILogger logger, T customObject, Exception? exception)
  {
    var type = customObject.GetType();

    if (type == typeof(string))
    {
      var customMessage = GetStringObject(customObject.ToString());

      logger.Log(LogLevel.Error, exception, customMessage);

      return;
    }

    var customMessageJson = GetObjectLogging(customObject);

    logger.Log(LogLevel.Error, exception, customMessageJson);
  }

  /// <summary>
  /// Logs an error message with a descriptive message, a custom object, and an associated exception. 
  /// If the object is a string, it is logged as-is; otherwise, it is serialized to JSON.
  /// A tracker ID is automatically included in the log.
  /// </summary>
  /// <typeparam name="T">The type of the custom object to log.</typeparam>
  /// <param name="logger">The <see cref="ILogger"/> instance used for logging.</param>
  /// <param name="message">A custom message to include with the log entry.</param>
  /// <param name="customObject">
  /// The object to log. If it is a <see cref="string"/>, it will be logged directly;
  /// otherwise, it will be serialized to JSON before logging.
  /// </param>
  /// <param name="exception">The <see cref="Exception"/> to be logged alongside the message and object.</param>
  public static void LogErrorCustom<T>(this ILogger logger, string message, T customObject, Exception? exception)
  {
    var customMessageJson = GetMessageObject(message, customObject);

    logger.Log(LogLevel.Error, exception, customMessageJson);
  }

  private static string GetMessageObject<T>(string message, T customObject)
  {
    var logObject = new Dictionary<string, object>
    {
      { "TrackerId", TrackedLogger.ExecutionName },
      { "Message", message },
      { "CustomObject", customObject }
    };

    return JsonSerializer.Serialize(logObject, _jsonSerializerOptions);
  }

  private static string GetStringObject(string message)
  {
    var logObject = new Dictionary<string, object>
    {
      { "TrackerId", TrackedLogger.ExecutionName },
      { "Message", message }
    };

    return JsonSerializer.Serialize(logObject, _jsonSerializerOptions);
  }

  private static string GetObjectLogging<T>(T customObject)
  {
    var logObject = new Dictionary<string, object>
    {
      { "TrackerId", TrackedLogger.ExecutionName },
      { "CustomObject", customObject }
    };

    return JsonSerializer.Serialize(logObject, _jsonSerializerOptions);
  }
}