using Dust.Compiler.Lexer;

namespace Dust.Compiler.Parser.SyntaxTree.Expressions
{
  public class LiteralExpression<T> : Expression
  {
    public SyntaxToken Token { get; }
    public T Value { get; }

    public LiteralExpression(SyntaxToken token, T value)
    {
      Token = token;
      Value = value;
    }
  }
}