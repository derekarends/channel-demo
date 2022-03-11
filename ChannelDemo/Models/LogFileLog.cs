namespace ChannelDemo.Models
{
  /// <summary>
  /// Model used for individual logs in a log file
  /// </summary>
  public class LogFileLog
  {
    public string Id { get; set; }
    public string Email { get; set; }
    public string Message { get; set; }
  }
}