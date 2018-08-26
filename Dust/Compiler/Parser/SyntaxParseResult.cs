using System.Collections.Generic;
using Dust.Compiler.Diagnostics;
using Dust.Compiler.Parser.AbstractSyntaxTree;

namespace Dust.Compiler.Parser
{
  public class SyntaxParseResult
  {
    public Node Node { get; }
    public List<Diagnostic> Diagnostics { get; }

    public SyntaxParseResult(Node node, List<Diagnostic> diagnostics)
    {
      Node = node;
      Diagnostics = diagnostics;
    }
  }
}