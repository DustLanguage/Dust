using Dust.Compiler.Types;

namespace Dust.Compiler.Parser.SyntaxTree
{
  public class VariableDeclaration : SyntaxNode
  {
    public bool IsMutable { get; }
    public string Name { get; }
    public DustType Type { get; }
    public SyntaxNode Initializer { get; }

    public VariableDeclaration(string name, bool isMutable, DustType type, SyntaxNode initializer, SourceRange range)
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