using System.Security.Cryptography;
using System.Text;

namespace Iagv.DrawingToKml;

public static class Hash
{
  public static string GetHash(string path)
  {
    using var md5 = MD5.Create();
    using var stream = File.OpenRead(path);
    var hashInBytes = md5.ComputeHash(stream);

    return Convert.ToBase64String(hashInBytes);
  }
}