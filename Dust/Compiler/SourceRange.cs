namespace Dust.Compiler
{
  public class SourceRange
  {
    public SourcePosition Start { get; }
    public SourcePosition End { get; }

    public SourceRange(SourcePosition start, SourcePosition end)
    {
      Start = start;
      End = end;
    }
  }
}