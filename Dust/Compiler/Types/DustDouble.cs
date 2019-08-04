using System;

namespace Dust.Compiler.Types
{
  public class DustDouble : DustType
  {
    public override int Rank => 3;

    public DustDouble()
      : base("double", DustTypes.Number)
    {
    }

    public override Type ToNativeType()
    {
      return typeof(double);
    }
  }
}