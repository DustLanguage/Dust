using System;
using System.Collections.Generic;
using System.Diagnostics;
using Dust.Extensions;

namespace Dust.Syntax
{
  public class Lexer : IDisposable
  {
    private readonly StringReader source;

    public Lexer(string text)
    {
      Debug.Assert(text != null, "Text is null.");

      source = new StringReader(text);
    }

    public List<Token> Lex()
    {
      List<Token> tokens = new List<Token>();

      char? character = source.Text[0];

      while (true)
      {
        if (source.Position + 1 <= source.Text.Length)
        {
          Token token = LexCharacter(character.Value);

          character = source.Advance();

          if (token != null)
          {
            tokens.Add(token);
          }
          
          if (character == null)
          {
            break;
          }
        }
        else
        {
          break;
        }
      }

      return tokens;
    }

    private Token LexCharacter(char character)
    {
      switch (character)
      {
        case '(':
          return new Token(TokenKind.OpenParentheses);
        case ')':
          return new Token(TokenKind.CloseParentheses);
        case '{':
          return new Token(TokenKind.OpenBrace);
        case '}':
          return new Token(TokenKind.CloseBrace);
        case '[':
          return new Token(TokenKind.OpenBracket);
        case ']':
          return new Token(TokenKind.CloseBracket);
        case '+':
          if (source.Peek() == '=')
          {
            source.Advance();

            return new Token(TokenKind.PlusEquals);
          }

          if (source.Peek() == '+')
          {
            source.Advance();

            return new Token(TokenKind.PlusPlus);
          }

          return new Token(TokenKind.Plus);
        case '-':
          if (source.Peek() == '=')
          {
            source.Advance();

            return new Token(TokenKind.MinusEquals);
          }

          if (source.Peek() == '-')
          {
            source.Advance();


            return new Token(TokenKind.MinusMinus);
          }

          return new Token(TokenKind.Minus);
        case '*':
          if (source.Peek() == '=')
          {
            source.Advance();

            return new Token(TokenKind.AsteriskEquals);
          }

          if (source.Peek() == '*')
          {
            source.Advance();

            return new Token(TokenKind.AsteriskAsterisk);
          }

          return new Token(TokenKind.Asterisk);
        case '/':
          if (source.Peek() == '=')
          {
            source.Advance();

            return new Token(TokenKind.SlashEquals);
          }

          if (source.Peek() == '/')
          {
            source.Advance();

            return new Token(TokenKind.SlashSlash);
          }

          return new Token(TokenKind.Slash);
        default:
          if (char.IsDigit(character))
          {
            return LexNumbericLiteral();
          }

          if (char.IsLetter(character) || character == '_')
          {
            return LexIdentifierOrKeyword();
          }

          return null;
      }
    }


    private Token LexNumbericLiteral()
    {
      int startPosition = source.Position;
      char? character = source.Peek();

      bool dotFound = false;
      bool invalidDot = false;

      while (character != null && (char.IsDigit(character.Value) || character.Value == '.'))
      {
        if (character.Value == '.')
        {
          if (dotFound)
          {
            invalidDot = true;
          }

          dotFound = true;
        }

        character = source.Advance();
      }

      if (invalidDot)
      {
        source.Revert();

        return null;
      }

      return new Token(TokenKind.NumericLiteral)
      {
        Text = source.Text.SubstringRange(startPosition, startPosition == source.Position ? source.Position + 1 : source.Position)
      };
    }


    private Token LexIdentifierOrKeyword()
    {
      int startPosition = source.Position;
      char? character = source.Peek();

      bool invalidSymbol = false;

      while (true)
      {
        if (character != null && IsValidIdentiferOrKeywordCharacter(character.Value))
        {
          character = source.Advance();
        }
        else if (character == ' ' || character == null)
        {
          break;
        }
        else if (IsValidIdentiferOrKeywordCharacter(character.Value) == false)
        {
          invalidSymbol = true;
        }
      }

      if (invalidSymbol)
      {
        Console.WriteLine("Invalid symbol in identifier.");

        return null;
      }

      return new Token(TokenKind.Identifier)
      {
        Text = source.Text.SubstringRange(startPosition, startPosition == source.Position ? source.Position + 1 : source.Position)
      };
    }

    private static bool IsValidIdentiferOrKeywordCharacter(char character)
    {
      return char.IsLetterOrDigit(character) || character == '_';
    }

    public void Dispose()
    {
      source.Dispose();
    }
  }
}