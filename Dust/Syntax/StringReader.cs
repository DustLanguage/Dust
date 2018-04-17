using System;

namespace Dust.Syntax
{
  public class StringReader : IDisposable
  {
    public int Position { get; private set; }
    public string Text { get; }

    private int startPosition;

    public StringReader(string text)
    {
      Position = 0;
      Text = text;
    }

    public char? Peek()
    {
      if (Position + 1 > Text.Length - 1)
      {
        return null;
      }

      return Text[Position + 1];
    }

    public char PeekBack()
    {
      return Text[Position - 1];
    }

    public char? Advance()
    {
      Position++;

      if (IsAtEnd())
      {
        return null;
      }

      return Text[Position - 1];
    }

    public void Revert()
    {
      Position--;
    }
    
    public void StartLexeme()
    {
      startPosition = Position;
    }

    public bool IsAtEnd()
    {
      return Position > Text.Length;
    }

    public void Dispose()
    {
    }
  }
}