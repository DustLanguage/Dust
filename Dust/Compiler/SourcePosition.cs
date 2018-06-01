namespace Dust.Compiler
{
  public class SourcePosition
  {
    public string File { get; }
    public int Line { get; }
    public int Column { get; }

    public SourcePosition(int line, int column)
    {
      File = "";
      Line = line;
      Column = column;
    }

    public static SourcePosition operator +(SourcePosition left, SourcePosition right)
    {
      return new SourcePosition(right.Line, left.Column + right.Column);
    }

    public static SourcePosition operator +(SourcePosition left, int right)
    {
      return new SourcePosition(left.Line, left.Column + right);
    }

    public static SourcePosition operator -(SourcePosition left, SourcePosition right)
    {
      return new SourcePosition(right.Line, left.Column - right.Column);
    }

    public static SourcePosition operator -(SourcePosition left, int right)
    {
      return new SourcePosition(left.Line, left.Column - right);
    }

    public static SourcePosition operator ++(SourcePosition position)
    {
      return new SourcePosition(position.Line, position.Column + 1);
    }

    public static SourcePosition operator --(SourcePosition position)
    {
      return new SourcePosition(position.Line, position.Column - 1);
    }

    public static bool operator ==(SourcePosition left, SourcePosition right)
    {
      if ((object) left == null && (object) right == null)
      {
        return true;
      }

      return left?.Line == right?.Line && left.Column == right.Column;
    }

    public static bool operator !=(SourcePosition left, SourcePosition right)
    {
      return left == right == false;
    }

    public override string ToString()
    {
      return $"line {Line}, column {Column}";
    }

    public bool Equals(SourcePosition other)
    {
      return string.Equals(File, other.File) && Line == other.Line && Column == other.Column;
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
        int hashCode = (File != null ? File.GetHashCode() : 0);
        hashCode = (hashCode * 397) ^ Line;
        hashCode = (hashCode * 397) ^ Column;
        return hashCode;
      }
    }
  }
}