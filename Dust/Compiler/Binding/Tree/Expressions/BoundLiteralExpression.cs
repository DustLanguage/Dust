using Dust.Compiler.Types;

namespace Dust.Compiler.Binding.Tree.Expressions
{
  public class BoundLiteralExpression : BoundExpression
  {
    public object Value { get; }
    public override DustType Type { get; }

    public BoundLiteralExpression(object value)
    {
      Value = value;
      Type = DustTypes.TypeOf(value);
    }
  }
}