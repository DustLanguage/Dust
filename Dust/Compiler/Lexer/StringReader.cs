using System;
using Dust.Extensions;

namespace Dust.Compiler.Lexer
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
      if (Position + 1 >= Text.Length)
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

      return Text[Position];
    }

    public void Start()
    {
      Start(Position);
    }
    
    public void Start(int position)
    {
      startPosition = position;
    }

    public string GetText()
    {
      return Text.SubstringRange(startPosition, startPosition == Position ? Position + 1 : Position);
    }

    public void Revert()
    {
      Position--;
    }

    public bool IsAtEnd()
    {
      return Position >= Text.Length;
    }

    public void Dispose()
    {
    }
  }
}