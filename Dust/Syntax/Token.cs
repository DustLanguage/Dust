namespace Dust.Syntax
{
  public class Token
  {
    public TokenKind Kind { get; }
    public string Text { get; set; }
    
    public Token(TokenKind kind)
    {
      Kind = kind;
    }
  }
}