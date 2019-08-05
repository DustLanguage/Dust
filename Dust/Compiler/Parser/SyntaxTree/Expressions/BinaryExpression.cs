using Dust.Compiler.Lexer;

namespace Dust.Compiler.Parser.SyntaxTree.Expressions
{
  public class BinaryExpression : Expression
  {
    public Expression Left { get; }
    public SyntaxToken OperatorToken { get; }
    public Expression Right { get; }

    public BinaryExpression(Expression left, SyntaxToken operatorToken, Expression right)
    {
      Left = left;
      OperatorToken = operatorToken;
      Right = right;
    }
  }
}