using System;
using System.Text.Json;
using ChannelDemo.Models;

namespace ChannelDemo
{
  /// <summary>
  /// Consume log messages and write them to the console.
  /// </summary>
  public class LogMessageConsumer
  {
    /// <summary>
    /// Write given log messages to the console.
    /// </summary>
    /// <param name="messageCountModel">A Log message model</param>
    public void Print(LogMessageCountModel messageCountModel)
    {
      var options = new JsonSerializerOptions
      {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
      };
      
      Console.WriteLine(JsonSerializer.Serialize(messageCountModel, options));
    }
  }
}