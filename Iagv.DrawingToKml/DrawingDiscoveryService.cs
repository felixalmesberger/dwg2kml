using System.Text.RegularExpressions;

namespace Iagv.DrawingToKml;

public class DrawingDiscoveryService
{

  private readonly Regex drawingRegex;

  public DrawingDiscoveryService(string includedDrawingRegexPattern)
  {
    this.drawingRegex = new Regex(includedDrawingRegexPattern);
  }

  public IList<string> GetAllDrawings(string baseDirectory)
  {
    return Directory.EnumerateFiles(baseDirectory, "*.dwg", SearchOption.AllDirectories).Where(this.IsIncludedDrawing).ToList();
  }

  private bool IsIncludedDrawing(string path)
  {
    return this.drawingRegex.IsMatch(Path.GetFileName(path));
  }
}