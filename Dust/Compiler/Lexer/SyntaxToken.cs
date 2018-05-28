namespace Dust.Compiler.Lexer
{
  public class SyntaxToken
  {
    public SyntaxTokenKind Kind { get; set; }
    public string Text { get; set; }
    public SourcePosition Position { get; set; }
  }
}