using System.Collections.Generic;
using ChannelDemo.Models;
using FluentAssertions;
using Xunit;

namespace ChannelDemo.Tests
{
  public class GlobalCountConsumerTests
  {
    [Fact]
    public void SumMessages_NullModel_DoesntChangeCount()
    {
      // Arrange
      var consumer = new GlobalCountConsumer();
      // Act
      consumer.SumMessages(null);
      // Assert
      consumer.Total.Should().Be(0, "Because the model was null");
    }
    
    [Fact]
    public void SumMessages_NullTally_DoesntChangeCount()
    {
      // Arrange
      var consumer = new GlobalCountConsumer();
      // Act
      consumer.SumMessages(new LogMessageCountModel());
      // Assert
      consumer.Total.Should().Be(0, "Because the tally was null");
    }
    
    [Fact]
    public void SumMessages_OneTally_UpdatesCount()
    {
      // Arrange
      var consumer = new GlobalCountConsumer();
      var countModel = new LogMessageCountModel
      {
        Tally = new List<LogMessageEmailTally>
        {
          new LogMessageEmailTally
          {
            Total = 1
          }
        }
      };
      
      // Act
      consumer.SumMessages(countModel);
      // Assert
      consumer.Total.Should().Be(1, "Because there was one total in the list of tallies");
    }
    
    [Fact]
    public void SumMessages_ThreeTally_UpdatesCount()
    {
      // Arrange
      var consumer = new GlobalCountConsumer();
      var countModel = new LogMessageCountModel
      {
        Tally = new List<LogMessageEmailTally>
        {
          new LogMessageEmailTally
          {
            Total = 1
          },
          new LogMessageEmailTally
          {
            Total = 3
          },
          new LogMessageEmailTally
          {
            Total = 5
          },
        }
      };
      
      // Act
      consumer.SumMessages(countModel);
      // Assert
      consumer.Total.Should().Be(9, "Because there was three tallies to sum");
    }
  }
}