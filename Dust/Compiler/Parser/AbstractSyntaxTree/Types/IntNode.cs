namespace Dust.Compiler.Parser.AbstractSyntaxTree.Types
{
  public class IntNode : Node
  {
    public int Value { get; }
   
    public IntNode(int value)
    {
      Value = value;
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