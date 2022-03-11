using System.IO;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ChannelDemo
{
  /// <summary>
  /// Used to get all files in a given directory and put them on a channel
  /// </summary>
  public class FileProducer
  {
    private readonly IFileService _fileService;

    public FileProducer(IFileService fileService)
    {
      _fileService = fileService;
    }
   
    /// <summary>
    /// Create a file producer to return all files from a given directory
    /// </summary>
    /// <param name="directory">Directory path</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns><see cref="ChannelReader{T}"/></returns>
    public ChannelReader<FileInfo> CreateProducer(string directory, CancellationToken ct)
    {
      var ch = Channel.CreateBounded<FileInfo>(100);
      if (string.IsNullOrWhiteSpace(directory))
      {
        ch.Writer.Complete();
        return ch;
      }
      
      Task.Run(async () =>
      {
        foreach (var file in _fileService.GetFiles(directory))
        {
          await ch.Writer.WriteAsync(file, ct);
        }

        ch.Writer.Complete();
      }, ct);
      
      return ch.Reader;
    }
  }
}