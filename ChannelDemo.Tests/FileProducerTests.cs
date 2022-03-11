using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace ChannelDemo.Tests
{
  public class FileProducerTests
  { 
    [Fact]
    public async Task CreateProducer_NullDirectory_ReturnsEmpty()
    {
      // Arrange
      var fileService = A.Fake<IFileService>();
      var fileProducer = new FileProducer(fileService);

      // Act
      var channel = fileProducer.CreateProducer(null, CancellationToken.None);
      var count = await Helpers.GetChannelCount(channel);

      // Assert
      count.Should().Be(0, "Because null was passed for directory");
    }
    
    [Fact]
    public async Task CreateProducer_EmptyDirectory_ReturnsEmpty()
    {
      // Arrange
      var fileService = A.Fake<IFileService>();
      var getFilesCaller = A.CallTo(() => fileService.GetFiles(A<string>._));
      getFilesCaller.Returns(new List<FileInfo>());
      
      var fileProducer = new FileProducer(fileService);

      // Act
      var channel = fileProducer.CreateProducer("directoryPath", CancellationToken.None);
      var count = await Helpers.GetChannelCount(channel);

      // Assert
      count.Should().Be(0, "Because there were not files in directory");
      getFilesCaller.MustHaveHappenedOnceExactly();
    }
    
    [Fact]
    public async Task CreateProducer_DirectoryWithMultiple_ReturnsMultiple()
    {
      // Arrange
      var files = new List<FileInfo>
      {
        new FileInfo("file1"),
        new FileInfo("file2"),
        new FileInfo("file3")
      };
      
      var fileService = A.Fake<IFileService>();
      var getFilesCaller = A.CallTo(() => fileService.GetFiles(A<string>._));
      getFilesCaller.Returns(files);
      
      var fileProducer = new FileProducer(fileService);

      // Act
      var channel = fileProducer.CreateProducer("directoryPath", CancellationToken.None);
      var count = await Helpers.GetChannelCount(channel);

      // Assert
      count.Should().Be(3, "Because there were three files in directory");
      getFilesCaller.MustHaveHappenedOnceExactly();
    }
  }
}