using System.Collections.Generic;

namespace Dust.Compiler.Parser.AbstractSyntaxTree
{
  public class CodeBlockNode : Node 
  {
    public List<Node> Children { get; }

    public CodeBlockNode(SourceRange range = null)
    {
      Range = range;
      
      Children = new List<Node>();
    }
    
    public override void Visit(IVisitor visitor)
    {
      visitor.Accept(this);
    }

    public override void VisitChildren(IVisitor visitor)
    {
      foreach (Node node in Children)
      {
        node.Visit(visitor);
      }
    }

  }
}