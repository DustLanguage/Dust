using Dust.Compiler.Types;

namespace Dust.Compiler.Binding.Tree.Expressions
{
  public class BoundLiteralExpression : BoundExpression
  {
    public BoundLiteralExpression(object value)
    {
      Value = DustTypes.FromNative(value);
    }
  }
}