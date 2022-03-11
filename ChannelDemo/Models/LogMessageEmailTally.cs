namespace ChannelDemo.Models
{
  /// <summary>
  /// Model to key track of emails and total count in an individual log
  /// </summary>
  public class LogMessageEmailTally
  {
    public string Email { get; set; }
    public int Total { get; set; }
  }
}