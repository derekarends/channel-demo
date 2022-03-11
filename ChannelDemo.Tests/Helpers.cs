using System.Threading.Channels;
using System.Threading.Tasks;

namespace ChannelDemo.Tests
{
  public class Helpers
  {
    public static async Task<int> GetChannelCount<T>(ChannelReader<T> ch)
    {
      var itemsInChannel = 0;
      await foreach (var _ in ch.ReadAllAsync())
      {
        itemsInChannel += 1;
      }

      return itemsInChannel;
    }
  }
}