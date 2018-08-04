namespace Dust.Compiler.Parser.AbstractSyntaxTree
{
  public class PropertyDeclarationNode : Node
  {
    public bool IsMutable { get; }
    public string Name { get; }

    public PropertyDeclarationNode(string name, bool isMutable,  SourceRange range)
    {
      Name = name;
      IsMutable = isMutable;
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