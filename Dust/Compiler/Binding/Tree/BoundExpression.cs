using Dust.Compiler.Types;

namespace Dust.Compiler.Binding.Tree
{
  public abstract class BoundExpression : BoundNode
  {
    public abstract DustType Type { get; }
  }
}