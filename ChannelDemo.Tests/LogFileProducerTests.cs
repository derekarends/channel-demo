using System.IO;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace ChannelDemo.Tests
{
  public class LogFileProducerTests
  { 
    [Fact]
    public async Task CreateProducer_NullChannel_ReturnsEmpty()
    {
      // Act
      var channel = LogFileProducer.CreateProducer(null, CancellationToken.None);
      var count = await Helpers.GetChannelCount(channel);

      // Assert
      count.Should().Be(0, "Because the channel was null");
    }

    [Fact]
    public async Task CreateProducer_EmptyChannel_ReturnsEmpty()
    {
      // Arrange
      var files = Channel.CreateBounded<FileInfo>(100);
      files.Writer.Complete();
      
      // Act
      var channel = LogFileProducer.CreateProducer(files.Reader, CancellationToken.None);
      var count = await Helpers.GetChannelCount(channel);

      // Assert
      count.Should().Be(0, "Because there were not files in channel");
    }
    
    [Fact]
    public async Task CreateProducer_InvalidFileLocation_ReturnsEmpty()
    {
      // Arrange
      var files = Channel.CreateBounded<FileInfo>(100);
      await files.Writer.WriteAsync(new FileInfo("./Data/invalid.json"));
      files.Writer.Complete();
      
      // Act
      var channel = LogFileProducer.CreateProducer(files.Reader, CancellationToken.None);
      var count = await Helpers.GetChannelCount(channel);

      // Assert
      count.Should().Be(0, "Because there was an invalid location provided");
    }
    
    [Fact]
    public async Task CreateProducer_InvalidFileLocationContinuesReading_ReturnsOne()
    {
      // Arrange
      var files = Channel.CreateBounded<FileInfo>(100);
      await files.Writer.WriteAsync(new FileInfo("./Data/invalid.json"));
      await files.Writer.WriteAsync(new FileInfo("./Data/logs_0.json"));
      files.Writer.Complete();
      
      // Act
      var channel = LogFileProducer.CreateProducer(files.Reader, CancellationToken.None);
      var count = await Helpers.GetChannelCount(channel);

      // Assert
      count.Should().Be(1, "Because there was one valid location provided");
    }
    
    [Fact]
    public async Task CreateProducer_InvalidFileJson_ReturnsEmpty()
    {
      // Arrange
      var files = Channel.CreateBounded<FileInfo>(100);
      await files.Writer.WriteAsync(new FileInfo("./Data/BadJson.json"));
      files.Writer.Complete();
      
      // Act
      var channel = LogFileProducer.CreateProducer(files.Reader, CancellationToken.None);
      var count = await Helpers.GetChannelCount(channel);

      // Assert
      count.Should().Be(0, "Because the json data was bad");
    }
    
    [Fact]
    public async Task CreateProducer_OneFile_ReturnsOneLog()
    {
      // Arrange
      var files = Channel.CreateBounded<FileInfo>(100);
      await files.Writer.WriteAsync(new FileInfo("./Data/logs_0.json"));
      files.Writer.Complete();
      
      // Act
      var channel = LogFileProducer.CreateProducer(files.Reader, CancellationToken.None);
      var count = await Helpers.GetChannelCount(channel);

      // Assert
      count.Should().Be(1, "Because there was one file in channel");
    }
    
    [Fact]
    public async Task CreateProducer_TwoFiles_ReturnsTwoLog()
    {
      // Arrange
      var files = Channel.CreateBounded<FileInfo>(100);
      await files.Writer.WriteAsync(new FileInfo("./Data/logs_0.json"));
      await files.Writer.WriteAsync(new FileInfo("./Data/logs_1.json"));
      files.Writer.Complete();
      
      // Act
      var channel = LogFileProducer.CreateProducer(files.Reader, CancellationToken.None);
      var count = await Helpers.GetChannelCount(channel);

      // Assert
      count.Should().Be(2, "Because there was two files in channel");
    }
  }
}