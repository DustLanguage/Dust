using System.Collections.Generic;

namespace Dust.Compiler.Parser.AbstractSyntaxTree
{
  public class PropertyDeclarationNode : Node
  {
    public bool IsMutable { get; }
    public string Name { get; }

    public PropertyDeclarationNode(string name, bool isMutable)
    {
      Name = name;
      IsMutable = isMutable;
    }

    public override void Visit(IVisitor visitor)
    {
      visitor.Accept(this);
    }

    public override void VisitChildren(IVisitor visitor)
    {
    }
  }
}