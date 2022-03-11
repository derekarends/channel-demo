using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using ChannelDemo.Models;

namespace ChannelDemo
{
  /// <summary>
  /// Used to produce log files for consumers
  /// </summary>
  public static class LogFileProducer
  {
    /// <summary>
    /// Given a channel of file infos, read the file, and deserialize it to a LogFileModel
    /// </summary>
    /// <param name="filesChannel">Channel of files</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns><see cref="ChannelReader{T}"/></returns>
    public static ChannelReader<LogFileModel> CreateProducer(ChannelReader<FileInfo> filesChannel, CancellationToken ct)
    {
      var ch = Channel.CreateBounded<LogFileModel>(100);
      if (filesChannel is null)
      {
        ch.Writer.Complete();
        return ch;
      }
      
      Task.Run(async () =>
      {
        await foreach (var file in filesChannel.ReadAllAsync(ct))
        {
          if (!file.Exists)
            continue;
          
          await using var fileStream = file.OpenRead();
          var logFile = await JsonSerializer.DeserializeAsync<LogFileModel>(fileStream, 
            new JsonSerializerOptions {PropertyNameCaseInsensitive = true}, ct);
          if (logFile is null)
            continue;
          
          await ch.Writer.WriteAsync(logFile, ct);
        }

        ch.Writer.Complete();
      }, ct);
      
      return ch.Reader;
    }
  }
}