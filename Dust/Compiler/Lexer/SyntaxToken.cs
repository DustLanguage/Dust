using System.Linq;

namespace Dust.Compiler.Lexer
{
  public class SyntaxToken
  {
    public SyntaxTokenKind Kind { get; set; }
    public string Text { get; set; }
    public string Lexeme { get; set; }

    public static SyntaxToken Invalid { get; } = new SyntaxToken
    {
      Kind = SyntaxTokenKind.Invalid
    };
    
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

    public bool Is(SyntaxTokenKind kind) => kind == Kind;
    public bool IsOr(params SyntaxTokenKind[] kinds) => kinds.Contains(Kind);

    public bool Isnt(SyntaxTokenKind kind) => !Is(kind);
    public bool IsntOr(params SyntaxTokenKind[] kinds) => !IsOr(kinds);
  }
}