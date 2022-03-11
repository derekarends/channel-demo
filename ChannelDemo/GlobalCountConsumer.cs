using System;
using System.Linq;
using ChannelDemo.Models;

namespace ChannelDemo
{
  /// <summary>
  /// Keep track of the total log message counts
  /// </summary>
  public class GlobalCountConsumer
  {
    public int Total { get; private set; }

    /// <summary>
    /// Append the total of all email counts
    /// </summary>
    /// <param name="messageCountModel">A message count model</param>
    public void SumMessages(LogMessageCountModel messageCountModel)
    {
      Total += messageCountModel?.Tally?.Sum(s => s.Total) ?? 0;
    }

    /// <summary>
    /// Print the total count
    /// </summary>
    public void PrintCount()
    {
      Console.WriteLine($"Total Count: {Total}");
    }
  }
}