using System.Diagnostics;
using System.Text;

namespace Iagv.DrawingToKml;

public class DwgKmlConverter
{
  public string ProfilePath { get; set; } = "profile.epf";

  public string AutoCADPath { get; set; }= "C:\\Program Files\\Autodesk\\AutoCAD 2023\\acad.exe";

  public void ConvertToKml(string source, string destination)
  {
    var script = this.GenerateConversionScript(source, destination);
    this.ExecuteScriptOnDrawing(source, script);
  }

  private string GenerateConversionScript(string source, string destination)
  {
    var scriptBuilder = new StringBuilder();

    var absoluteProfilePath = Path.GetFullPath(this.ProfilePath).Replace("\\", "\\\\");
    var absoluteDestination = Path.GetFullPath(destination).Replace("\\", "\\\\");

    // erzeuge skript um datei zu exportieren und AutoCAD wieder zu schließen

    scriptBuilder.AppendLine($"(command \"-mapexport\" \"OGCKML\" \"{absoluteDestination}\" \"J\" \"{absoluteProfilePath}\" \"F\")");
    scriptBuilder.AppendLine("quit");

    return scriptBuilder.ToString();
  }

  private void ExecuteScriptOnDrawing(string drawing, string script)
  {
    var scriptFilename = Path.Combine(Path.GetTempPath(), $"{Path.GetFileNameWithoutExtension(drawing)}.scr");

    File.WriteAllText(scriptFilename, script);

    var absoluteDrawingPath = Path.GetFullPath(drawing);

    try
    {
      var acad = Process.Start(this.AutoCADPath, $"/ld \"C:\\Program Files\\Autodesk\\AutoCAD 2023\\AecBase.dbx\" /p \"<<C3D_Metric>>\" /product C3D /language de-DE /nologo /i \"{absoluteDrawingPath}\" /b \"{scriptFilename}\"");

      var hasExited = acad.WaitForExit(3 * 60 * 1000);

      if (!hasExited)
        acad.Kill();
      else
        Console.WriteLine("AutoCAD did not respond within 60 seconds and had to be killed. Conversion might have failed.");
    }
    finally
    {
      // delete
      File.Delete(scriptFilename);
    }
  }
}