using Dust.Compiler.Lexer;

namespace Dust.Compiler.Parser.SyntaxTree.Expressions
{
  public class LiteralExpression : Expression
  {
    public SyntaxToken Token { get; }
    public object Value { get; }

    public LiteralExpression(SyntaxToken token, object value)
    {
      Token = token;
      Value = value;
    }
  }
}