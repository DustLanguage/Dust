using Dust.Compiler.Types;

namespace Dust.Compiler.Parser
{
  public class FunctionParameter
  {
    public string Name { get; }
    public DustType Type { get; }
    public bool IsMutable { get; }

    public FunctionParameter(string name, DustType type, bool isMutable)
    {
      Name = name;
      Type = type;
      IsMutable = isMutable;
    }
  }
}