namespace Dust.Compiler.Parser.AbstractSyntaxTree.Expressions
{
  public class LiteralExpression<T> : Expression
  {
    public T Value { get; }

    public LiteralExpression(T value)
    {
      Value = value;
    }

    public override void Visit(IVisitor visitor)
    {
    }

    public override void VisitChildren(IVisitor visitor)
    {
    }
  }
}