namespace Dust.Compiler.Lexer
{
  public class SyntaxToken
  {
    public SyntaxTokenKind Kind { get; set; }
    public string Text { get; set; }

    public SourcePosition Position
    {
      get
      {
        if (position == null)
        {
          return position = Range.Start;
        }

        return position;
      }
      set => position = value;
    }

    public SourceRange Range
    {
      get
      {
        if (range == null)
        {
          return range = new SourceRange(position, position);
        }

        return range;
      }
      set => range = value;
    }

    private SourcePosition position;
    private SourceRange range;
  }
}