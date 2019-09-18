namespace Dust.Compiler.Types
{
  public class DustNumber : DustObject
  {
    public DustNumber()
      : base("number")
    {
    }

    protected DustNumber(string typeName)
      : base(typeName, DustTypes.Number)
    {
    }
  }
}