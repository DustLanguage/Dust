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
    
    public override void Visit(IVisitor visitor)
    {
      visitor.Accept(this);
    }

    public override void VisitChildren(IVisitor visitor)
    {
      foreach (SyntaxNode node in Children)
      {
        node.Visit(visitor);
      }
    }
  }
}