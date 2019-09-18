using Dust.Compiler.Types;

namespace Dust.Compiler.Binding.Tree
{
  public abstract class BoundExpression : BoundNode
  {
    public DustObject Value { get; protected set; }
    public virtual DustType Type => Value;
  }
}