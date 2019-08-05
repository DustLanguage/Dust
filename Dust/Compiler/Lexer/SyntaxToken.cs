using System.Linq;
using Dust.Compiler.Parser.SyntaxTree;

namespace Dust.Compiler.Lexer
{
  public class SyntaxToken : SyntaxNode
  {
    public static SyntaxToken Invalid { get; } = new SyntaxToken
    {
      Kind = SyntaxTokenKind.Invalid
    };

    public SyntaxTokenKind Kind { get; set; }
    public string Text { get; set; }
    public string Lexeme { get; set; }
    public SourcePosition Position { get; set; }
    public sealed override SourceRange Range => new SourceRange(Position, Position + Lexeme.Length - 1);

    public bool Is(SyntaxTokenKind kind) => kind == Kind;
    public bool IsOr(params SyntaxTokenKind[] kinds) => kinds.Contains(Kind);

    public bool Isnt(SyntaxTokenKind kind) => !Is(kind);
    public bool IsntOr(params SyntaxTokenKind[] kinds) => !IsOr(kinds);

    public SyntaxToken(SourceRange range = null)
    {
      Range = range;
    }
  }
}