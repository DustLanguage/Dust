using Dust.Compiler.Parser.SyntaxTree;

namespace Dust.Compiler.Binding.Tree
{
  public class BoundExpressionStatement : BoundStatement
  {
    public BoundExpression Expression { get; }

    public BoundExpressionStatement(BoundExpression expression)
    {
      Expression = expression;
    }
  }
}