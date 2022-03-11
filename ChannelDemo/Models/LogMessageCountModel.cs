using System.Collections.Generic;

namespace ChannelDemo.Models
{
  /// <summary>
  /// Model used to track the log and the individual log message tallies
  /// </summary>
  public class LogMessageCountModel
  {
    public string LogId { get; set; }
    public IEnumerable<LogMessageEmailTally> Tally { get; set; }
  }
}