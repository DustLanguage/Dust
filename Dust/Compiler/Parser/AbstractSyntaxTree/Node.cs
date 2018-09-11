using System;

namespace Dust.Compiler.Parser.AbstractSyntaxTree
{
  public abstract class Node : IEquatable<Node>
  {
    public SourceRange Range { get; set; }

    public abstract void Visit(IVisitor visitor);
    public abstract void VisitChildren(IVisitor visitor);

    public bool Equals(Node other)
    {
      if (ReferenceEquals(null, other)) return false;
      
      return ReferenceEquals(this, other) || Equals(Range, other.Range);
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      
      return obj.GetType() == GetType() && Equals((Node) obj);
    }

    public override int GetHashCode()
    {
      return Range != null ? Range.GetHashCode() : 0;
    }
  }
}