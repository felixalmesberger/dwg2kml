using Iagv.DrawingToKml;

Console.WriteLine("Drawings -> KML");

var commandLine = new CommandLine(args);

var hasSource = commandLine.TryGetValue("source", out var source);
var hasProfile = commandLine.TryGetValue("profile", out var profile);
var hasDestination = commandLine.TryGetValue("destination", out var destination);
var hasIncludedDrawingRegexPattern =
  commandLine.TryGetValue("included-drawing-regex", out var includedDrawingRegexPattern);

var showHelp = commandLine.IsFlagSet("help");

if (showHelp)
{
  ShowHelp();
  return;
}


if (!hasIncludedDrawingRegexPattern)
  includedDrawingRegexPattern = ".*";

if (!hasSource || !hasProfile || !hasDestination)
{
  ShowHelp();
  return;
}

var databasePath = "drawings.json";

if (!Directory.Exists(destination))
  Directory.CreateDirectory(destination);

var discoveryService = new DrawingDiscoveryService(includedDrawingRegexPattern);
var database = DrawingDatabase.FromFile(databasePath);
var converter = new DwgKmlConverter()
{
  ProfilePath = profile
};

Console.WriteLine("Discovering Drawings");
Console.WriteLine("This might take a while");

var drawings = discoveryService.GetAllDrawings(source);

foreach (var drawing in drawings)
{
  try
  {
    if (!database.IsConversionRequired(drawing))
    {
      Console.WriteLine($"Skipping already processed drawing {drawing}");
      continue;
    }

    Console.WriteLine($"Processing drawing '{drawing}'");

    var tempDwgPath = Path.Combine(destination, Path.GetFileName(drawing));
    var kmlPath = Path.ChangeExtension(tempDwgPath, "kml");

    if (File.Exists(tempDwgPath))
      File.Delete(tempDwgPath);

    File.Copy(drawing, tempDwgPath);
    converter.ConvertToKml(tempDwgPath, kmlPath);

    database.AddOrUpdate(drawing);
    database.Save();
  }
  catch (Exception ex)
  {
    Console.WriteLine($"Error:{ex}");
  }
}

void ShowHelp()
{
  Console.WriteLine("--source <path> --profile <epf file> --destination <path>");
  Console.Write("[Optional] --included-drawing-regex <patten for files that will be included>");
}