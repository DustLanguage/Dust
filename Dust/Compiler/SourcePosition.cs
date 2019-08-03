using System;

namespace Dust.Compiler
{
  public class SourcePosition : IEquatable<SourcePosition>
  {
    public string File { get; }
    public int Line { get; }
    public int Position { get; }

    public SourcePosition(int line, int position)
    {
      File = "";
      Line = line;
      Position = position;
    }

    public static SourcePosition operator +(SourcePosition left, SourcePosition right)
    {
      return new SourcePosition(right.Line, left.Position + right.Position);
    }

    public static SourcePosition operator +(SourcePosition left, int right)
    {
      return new SourcePosition(left.Line, left.Position + right);
    }

    public static SourcePosition operator -(SourcePosition left, SourcePosition right)
    {
      return new SourcePosition(right.Line, left.Position - right.Position);
    }

    public static SourcePosition operator -(SourcePosition left, int right)
    {
      return new SourcePosition(left.Line, left.Position - right);
    }

    public static SourcePosition operator ++(SourcePosition position)
    {
      return new SourcePosition(position.Line, position.Position + 1);
    }

    public static SourcePosition operator --(SourcePosition position)
    {
      return new SourcePosition(position.Line, position.Position - 1);
    }

    public static bool operator ==(SourcePosition left, SourcePosition right)
    {
      if ((object) left == null && (object) right == null)
      {
        return true;
      }

      return left?.Line == right?.Line && left.Position == right.Position;
    }

    public static bool operator !=(SourcePosition left, SourcePosition right)
    {
      return left == right == false;
    }

    public override string ToString()
    {
      return $"line {Line}, column {Position}";
    }

    public bool Equals(SourcePosition other)
    {
      if ((object) other == null)
      {
        return false;
      }

      return string.Equals(File, other.File) && Line == other.Line && Position == other.Position;
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      return obj.GetType() == GetType() && Equals((SourcePosition) obj);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        int hashCode = File != null ? File.GetHashCode() : 0;
        hashCode = (hashCode * 397) ^ Line;
        hashCode = (hashCode * 397) ^ Position;
        return hashCode;
      }
    }
  }
}