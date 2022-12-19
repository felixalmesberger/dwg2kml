namespace Iagv.DrawingToKml;

/// <summary>
/// Simple Command Line Argument Parser
/// Parses CommandLine in Format app.exe -flag -key value
/// </summary>
public class CommandLine
{

  private readonly string[] args;

  public CommandLine(string[] args)
  {
    this.args = args ?? throw new ArgumentNullException(nameof(args));
  }

  /// <summary>
  /// Checks if a flag -name is set
  /// </summary>
  /// <param name="name"></param>
  /// <returns></returns>
  public bool IsFlagSet(string name)
  {
    return this.args.Any(x => x.ToLower() == $"-{name.ToLower()}");
  }

  /// <summary>
  /// If a value in format -name value is specified returns true and value
  /// </summary>
  public bool TryGetValue(string name, out string value)
  {
    var indexOfArgument = this.args.Select(x => x.ToLower().TrimStart('-'))
      .ToList()
      .IndexOf(name);

    value = null;

    if (indexOfArgument < 0)
      return false;

    if (this.args.Length < indexOfArgument + 1)
      return false;

    value = this.args[indexOfArgument + 1].TrimStart('\"').TrimEnd('\"');
    return true;
  }
}