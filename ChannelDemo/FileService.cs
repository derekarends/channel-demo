using System.Collections.Generic;
using System.IO;

namespace ChannelDemo
{
  /// <summary>
  /// Used to interact with file IO
  /// </summary>
  public interface IFileService
  {
    /// <summary>
    /// Get all files in a given directory
    /// </summary>
    /// <param name="directory">Directory path</param>
    /// <returns><see cref="IEnumerable{T}"/></returns>
    IEnumerable<FileInfo> GetFiles(string directory);
  }
  
  /// <inheritdoc />
  public class FileService : IFileService
  {
    /// <inheritdoc />
    public IEnumerable<FileInfo> GetFiles(string path)
    {
      var directory = new DirectoryInfo(path);
      if (!directory.Exists)
        return new List<FileInfo>();
      
      return directory.EnumerateFiles();
    }
  }
}