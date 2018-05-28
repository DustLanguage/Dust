namespace Dust.Compiler.Parser.AbstractSyntaxTree
{
  public abstract class Node
  {
    public SourceRange Range { get; set; }
    
    public abstract void Visit(IVisitor visitor);
    public abstract void VisitChildren(IVisitor visitor);
  }
}