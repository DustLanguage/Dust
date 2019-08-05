using System;

namespace Dust.Compiler.Types
{
  public class DustFloat : DustType
  {
    public override int Rank => 2;

    public DustFloat()
      : base("float", DustTypes.Number)
    {
    }

    public override Type ToNativeType()
    {
      return typeof(float);
    }
  }
}