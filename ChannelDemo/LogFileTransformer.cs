using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using ChannelDemo.Models;

namespace ChannelDemo
{
  /// <summary>
  /// Used to transform log file data into log message counts
  /// </summary>
  public static class LogFileTransformer
  {
    /// <summary>
    /// Given a log file channel take the data and transform the data into a log message count
    /// </summary>
    /// <param name="logFileModelChannel">Channel of log files</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns><see cref="ChannelReader{T}"/></returns>
    public static ChannelReader<LogMessageCountModel> CreateTransformer(ChannelReader<LogFileModel> logFileModelChannel,
      CancellationToken ct)
    {
      var ch = Channel.CreateBounded<LogMessageCountModel>(100);
      if (logFileModelChannel is null)
      {
        ch.Writer.Complete();
        return ch;
      }

      Task.Run(async () =>
      {
        await foreach (var logFileModel in logFileModelChannel.ReadAllAsync(ct))
        {
          var tallies = logFileModel.Logs
            .GroupBy(
              log => log.Email.Normalize(),
              (email, rows) => new LogMessageEmailTally
              {
                Email = email,
                Total = rows
                  .Distinct()
                  .Count()
              }
            );

          var result = new LogMessageCountModel
          {
            LogId = logFileModel.Id,
            Tally = tallies
          };

          await ch.Writer.WriteAsync(result, ct);
        }

        ch.Writer.Complete();
      }, ct);

      return ch.Reader;
    }
  }
}