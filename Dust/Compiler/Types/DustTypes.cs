namespace Dust.Compiler.Types
{
  public static class DustTypes
  {
    public static DustType Void => new DustVoid();
    public static DustType Int => new DustInt();

    public static DustType GetType(string typeName)
    {
      switch (typeName)
      {
        case "void":
          return Void;
        case "int":
          return Int;
      }

      return null;
    }
  }
}