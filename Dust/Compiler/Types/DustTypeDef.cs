using System.Collections.Generic;

namespace Dust.Compiler.Types
{
  public class DustType
  {
    public string TypeName { get; }

    protected Dictionary<string, DustObject> functions = new Dictionary<string, DustObject>();
    protected Dictionary<string, DustObject> properties = new Dictionary<string, DustObject>();
    
    public DustType(string typeName)
    {
      TypeName = typeName;
    }
  }
}