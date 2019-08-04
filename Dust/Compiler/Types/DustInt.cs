using System;

namespace Dust.Compiler.Types
{
  public class DustInt : DustType
  {
    public override int Rank => 1;

    public DustInt()
      : base("int", DustTypes.Number)
    {
    }

    public override Type ToNativeType()
    {
      return typeof(int);
    }
  }
}