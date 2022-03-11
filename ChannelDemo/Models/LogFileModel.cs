using System.Collections.Generic;

namespace ChannelDemo.Models
{
  /// <summary>
  /// Model for the individual log files
  /// </summary>
  public class LogFileModel
  {
    public string Id { get; set; }
    public IEnumerable<LogFileLog> Logs { get; set; }
  }
}