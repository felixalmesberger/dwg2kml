using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Iagv.DrawingToKml;

public class Drawing
{
  public string Path { get; set; }
  public string Hash { get; set; }
  public DateTime TimeStamp { get; set; }
}

public class DrawingDatabase
{
  public IList<Drawing> Drawings { get; set; } = new List<Drawing>();

  [JsonIgnore]
  public string Origin { get; private set; }

  public bool IsConversionRequired(string path)
  {
    var entry = this.Drawings.FirstOrDefault(x => x.Path == path);

    if (entry is null)
      return true;

    var hash = Hash.GetHash(path);

    return entry.Hash == hash;
  }

  public void AddOrUpdate(string path)
  {
    var entry = this.Drawings.FirstOrDefault(x => x.Path == path);

    if (entry is null)
    {
      entry = new Drawing()
      {
        Hash = Hash.GetHash(path),
        Path = path,
        TimeStamp = DateTime.Now
      };

      this.Drawings.Add(entry);
    }

    entry.Hash = Hash.GetHash(path);
  }

  private static readonly JsonSerializerOptions jsonSerializerOptions = new()
  {
    WriteIndented = true
  };

  public static DrawingDatabase FromFile(string path)
  {
    if (!File.Exists(path))
    {
      return new DrawingDatabase()
      {
        Origin = path
      };
    }

    var fromFile = JsonSerializer.Deserialize<DrawingDatabase>(File.ReadAllText(path), jsonSerializerOptions);
    fromFile.Origin = path;
    return fromFile;
  }

  public void Save()
  {
    var content = JsonSerializer.Serialize(this, jsonSerializerOptions);
    File.WriteAllText(this.Origin, content);
  }
}