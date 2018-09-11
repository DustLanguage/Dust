using System;
using System.Linq;

namespace Dust.Compiler
{
  public class SourceRange : IEquatable<SourceRange>
  {
    public SourcePosition Start { get; }
    public SourcePosition End { get; }

    public SourceRange(SourcePosition start, SourcePosition end)
    {
      Start = start;
      End = end;
    }

    public static SourceRange FromText(string text, int startLine = 0, int offset = 0)
    {
      return new SourceRange(new SourcePosition(startLine, offset), new SourcePosition(text.Count((c) => c == '\n') + startLine, offset + text.Length - 1));
    }

    public bool Equals(SourceRange other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      
      return Equals(Start, other.Start) && Equals(End, other.End);
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      
      return obj.GetType() == GetType() && Equals((SourceRange) obj);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        return ((Start != null ? Start.GetHashCode() : 0) * 397) ^ (End != null ? End.GetHashCode() : 0);
      }
    }
  }
}