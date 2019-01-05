namespace Dust.Compiler.Parser.AbstractSyntaxTree
{
  public class ExpressionStatement : Statement
  {
    public Expression Expression { get; }

    public ExpressionStatement(Expression expression)
    {
      Expression = expression;
      Range = expression.Range;
    }

    public override void Visit(IVisitor visitor)
    {
    }

    public override void VisitChildren(IVisitor visitor)
    {
    }
  }
}