using Dust.Compiler.Types;

namespace Dust.Compiler.Binding.Tree.Expressions
{
  public class BoundBinaryExpression : BoundExpression
  {
    public BoundExpression Left { get; }
    public BoundBinaryOperator Operator { get; }
    public BoundExpression Right { get; }
    public override DustType Type => Operator.ReturnType;

    public BoundBinaryExpression(BoundExpression left, BoundBinaryOperator @operator, BoundExpression right)
    {
      Left = left;
      Operator = @operator;
      Right = right;
    }
  }
}