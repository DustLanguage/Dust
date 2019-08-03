using System.Collections.Generic;

namespace Dust.Compiler.Parser.SyntaxTree
{
  public class CodeBlockNode : SyntaxNode 
  {
    public List<SyntaxNode> Children { get; }

    public CodeBlockNode(SourceRange range = null)
    {
      Range = range;
      
      Children = new List<SyntaxNode>();
    }
  }
}