using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ChannelDemo
{
  /// <summary>
  /// An application to loop through a log file directory, print our unique counts for emails
  /// and then print a global total
  /// </summary>
  internal static class Program
  {
    private static async Task Main(string[] args)
    {
      if (args.Length != 1)
      {
        Console.WriteLine("Please include directory of log files");
        return;
      }

      var directory = args[0]; 
      if(!Directory.Exists(directory))
      {
        Console.WriteLine($"Directory: [{directory}] does not exist.");
        return;
      }
      
      var ct = new CancellationToken();
      var fileService = new FileService();
      
      var fileProducer = new FileProducer(fileService);
      var fileChannel = fileProducer.CreateProducer(directory, ct);
      
      var logFileChannel = LogFileProducer.CreateProducer(fileChannel, ct);
      var logMessageChannel = LogFileTransformer.CreateTransformer(logFileChannel, ct);

      var logMessageConsumer = new LogMessageConsumer();
      var globalCountConsumer = new GlobalCountConsumer();

      await foreach (var logMessage in logMessageChannel.ReadAllAsync(ct))
      {
        logMessageConsumer.Print(logMessage);
        globalCountConsumer.SumMessages(logMessage);
      }

      globalCountConsumer.PrintCount();
    }
  }
}