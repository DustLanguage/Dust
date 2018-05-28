using Dust.Compiler.Parser.AbstractSyntaxTree;

namespace Dust.Compiler.Parser
{
  public class FunctionParameter
  {
    public string Name { get; }
    public Node Type { get; }
    public bool IsMutable { get; }

    public FunctionParameter(string name, Node type, bool isMutable)
    {
      Name = name;
      Type = type;
      IsMutable = isMutable;
    }
  }
}