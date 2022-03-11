using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using ChannelDemo.Models;
using FluentAssertions;
using Xunit;

namespace ChannelDemo.Tests
{
  public class LogFileTransformerTests
  {
    [Fact]
    public async Task CreateTransformer_NullChannel_ReturnsEmpty()
    {
      // Act
      var channel = LogFileTransformer.CreateTransformer(null, CancellationToken.None);
      var count = await Helpers.GetChannelCount(channel);

      // Assert
      count.Should().Be(0, "Because there was a null channel input");
    }

    [Fact]
    public async Task CreateTransformer_EmptyChannel_ReturnsEmpty()
    {
      // Arrange
      var files = Channel.CreateBounded<LogFileModel>(100);
      files.Writer.Complete();

      // Act
      var channel = LogFileTransformer.CreateTransformer(files.Reader, CancellationToken.None);
      var count = await Helpers.GetChannelCount(channel);

      // Assert
      count.Should().Be(0, "Because there were no files in channel");
    }

    [Fact]
    public async Task CreateTransformer_OneLogWithSixItems_ReturnsCorrectTallies()
    {
      // Arrange
      var logFileModel = new LogFileModel
      {
        Id = "1",
        Logs = new List<LogFileLog>
        {
          new LogFileLog
          {
            Email = "a@b.com",
          },
          new LogFileLog
          {
            Email = "a@b.com",
          },
          new LogFileLog
          {
            Email = "a@b.com",
          },
          new LogFileLog
          {
            Email = "b@b.com",
          },
          new LogFileLog
          {
            Email = "b@b.com",
          },
          new LogFileLog
          {
            Email = "c@b.com",
          }
        }
      };

      var logFile = Channel.CreateBounded<LogFileModel>(100);
      await logFile.Writer.WriteAsync(logFileModel);
      logFile.Writer.Complete();

      // Act
      var channel = LogFileTransformer.CreateTransformer(logFile.Reader, CancellationToken.None);
      var item = await channel.ReadAsync();
      
      // Asserts
      var tallies = item.Tally.ToList();
      tallies.Count.Should().Be(3, "There were three unique email addresses");
      
      var a = tallies[0];
      a.Email.Should().Be("a@b.com");
      a.Total.Should().Be(3);

      var b = tallies[1];
      b.Email.Should().Be("b@b.com");
      b.Total.Should().Be(2);
      
      var c = tallies[2];
      c.Email.Should().Be("c@b.com");
      c.Total.Should().Be(1);
    }
  }
}