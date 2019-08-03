namespace Dust.Compiler.Parser.SyntaxTree
{
  public class ExpressionStatement : Statement
  {
    public Expression Expression { get; }

    public ExpressionStatement(Expression expression)
    {
      Expression = expression;
    }
  }
}