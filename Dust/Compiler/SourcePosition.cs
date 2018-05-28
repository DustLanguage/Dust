namespace Dust.Compiler
{
  public class SourcePosition
  {
    public string File { get; }
    public int Line { get; }
    public int Column { get; }

    public SourcePosition(int line, int column)
    {
      File = "";
      Line = line;
      Column = column;
    }
  }
}