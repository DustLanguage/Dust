using System.Collections.Generic;
using Dust.Compiler.Diagnostics;
using Dust.Compiler.Parser.SyntaxTree;

namespace Dust.Compiler.Parser
{
  public class SyntaxParseResult
  {
    public CodeBlockNode Node { get; }
    public List<Diagnostic> Diagnostics { get; }

    public SyntaxParseResult(CodeBlockNode node, List<Diagnostic> diagnostics)
    {
      Node = node;
      Diagnostics = diagnostics;
    }
  }
}