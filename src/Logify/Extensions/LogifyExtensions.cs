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
  /// Logs information with a custom object. The custom object is either serialized to JSON or used as a string based on its type and adds a trackerId.
  /// </summary>
  /// <param name="logger">The logger instance used to log the information.</param>
  /// <param name="customObject">The custom object to be logged. If the object is of type string, it is logged as a message. Otherwise, it is serialized to JSON and logged.</param>
  /// <typeparam name="T">The type of the custom object being logged.</typeparam>
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
  /// Logs information with a custom object and an accompanying message. The custom object is either serialized to JSON or used as a string based on its type and adds a trackerId.
  /// </summary>
  /// <param name="logger">The logger instance used to log the information.</param>
  /// <param name="message">The message to be logged alongside the custom object.</param>
  /// <param name="customObject">The custom object to be logged. If the object is of type string, it is logged as a message. Otherwise, it is serialized to JSON and logged.</param>
  /// <typeparam name="T">The type of the custom object being logged.</typeparam>
  public static void LogInformationCustom<T>(this ILogger logger, string message, T customObject)
  {
    var customMessageJson = GetMessageObject(message, customObject);

    logger.Log(LogLevel.Information, customMessageJson);
  }

  /// <summary>
  /// Logs an error with a custom object and an accompanying exception. The custom object is either serialized to JSON or used as a string based on its type and adds a trackerId.
  /// </summary>
  /// <param name="logger">The logger instance used to log the error.</param>
  /// <param name="customObject">The custom object to be logged. If the object is of type string, it is logged as a message. Otherwise, it is serialized to JSON and logged.</param>
  /// <param name="exception">The exception associated with the error.</param>
  /// <typeparam name="T">The type of the custom object being logged.</typeparam>
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
  /// Logs an error with a custom object and an exception. The custom object is either serialized to JSON or used as a string based on its type and adds a trackerId.
  /// </summary>
  /// <param name="logger">The logger instance used to log the error.</param>
  /// <param name="message">The message to be logged alongside the custom object.</param>
  /// <param name="customObject">The custom object to be logged. If the object is of type string, it is logged as a message. Otherwise, it is serialized to JSON and logged.</param>
  /// <param name="exception">The exception to be logged along with the custom object.</param>
  /// <typeparam name="T">The type of the custom object being logged.</typeparam>
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