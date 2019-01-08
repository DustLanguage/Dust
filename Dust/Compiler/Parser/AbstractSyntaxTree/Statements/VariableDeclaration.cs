using Dust.Compiler.Types;

namespace Dust.Compiler.Parser.AbstractSyntaxTree
{
  public class VariableDeclaration : Node
  {
    public bool IsMutable { get; }
    public string Name { get; }
    public DustType Type { get; }
    public Node Initializer { get; }

    public VariableDeclaration(string name, bool isMutable, DustType type, Node initializer, SourceRange range)
    {
      Name = name;
      IsMutable = isMutable;
      Type = type;
      Initializer = initializer;
      Range = range;
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