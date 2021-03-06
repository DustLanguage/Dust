﻿using System;
using System.Diagnostics;
using System.Linq;
using Dust.Extensions;

namespace Dust.Compiler.Lexer
{
  public class StringReader
  {
    public int Position { get; private set; }

    public SourcePosition SourcePosition => GetSourcePosition(Position);

    public char Current => Text[Position];
    public string Text { get; }

    public StringReader(string text)
    {
      Position = -1;
      Text = text;
    }

    public char Peek()
    {
      if (Position + 1 >= Text.Length)
      {
        return default;
      }

      return Text[Position + 1];
    }

    public char PeekBack()
    {
      return Text[Position - 1];
    }

    public char Advance()
    {
      Position++;

      if (IsAtEnd())
      {
        return default;
      }

      return Text[Position];
    }

    public string Range(int start, int end)
    {
      return Text.SubstringRange(start, end);
    }

    public string Range(SourcePosition start, int end)
    {
      return Range(start.Position, end);
    }

    public string Range(SourceRange range)
    {
      return Range(range.Start.Position, range.End.Position);
    }

    public void Revert()
    {
      Position--;
    }

    public SourcePosition GetSourcePosition(int position)
    {
      Debug.Assert(position <= Text.Length);

      string text = Text.SubstringRange(0, position);

      int line = text.Count(character => character == '\n');
      int column = Math.Max(position - (text.LastIndexOf('\n') == -1 ? 0 : text.LastIndexOf('\n') + 1), 0);

      return new SourcePosition(line, column);
    }

    public bool IsAtEnd()
    {
      return Position >= Text.Length;
    }
  }
}