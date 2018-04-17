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
      
      char? character;

      while ((character = source.Advance()) != null)
      {
        Token token = LexCharacter(character.Value);

        if (token != null)
        {
          tokens.Add(token);
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
            return new Token(TokenKind.PlusEquals);
          }

          if (source.Peek() == '+')
          {
            return new Token(TokenKind.PlusPlus);
          }

          return new Token(TokenKind.Plus);
        case '-':
          if (source.Peek() == '=')
          {
            return new Token(TokenKind.MinusEquals);
          }

          if (source.Peek() == '-')
          {
            return new Token(TokenKind.MinusMinus);
          }

          return new Token(TokenKind.Minus);
        case '*':
          if (source.Peek() == '=')
          {
            return new Token(TokenKind.AsteriskEquals);
          }

          if (source.Peek() == '*')
          {
            return new Token(TokenKind.AsteriskAsterisk);
          }

          return new Token(TokenKind.Asterisk);
        case '/':
          if (source.Peek() == '=')
          {
            return new Token(TokenKind.SlashEquals);
          }

          if (source.Peek() == '/')
          {
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
      int startPosition = source.Position - 1;
      char? character = source.Advance();

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
      
      source.Revert();

      if (invalidDot)
      {
        source.Revert();

        return null;
      }

      character = source.PeekBack();

      if ((char.IsDigit(character.Value) || character.Value == '.') == false)
      {
        source.Revert();
      }

      return new Token(TokenKind.NumericLiteral)
      {
        Text = source.Text.SubstringRange(startPosition, source.Position)
      };
    }


    private Token LexIdentifierOrKeyword()
    {
      int startPosition = source.Position - 1;
      char? character = source.Advance();

      while (character != null && IsValidIdentiferOrKeywordCharacter(character.Value))
      {
        character = source.Advance();
      }

      if (character != null && IsValidIdentiferOrKeywordCharacter(character.Value) == false)
      {
        Console.WriteLine("Invalid symbol in identifier.");

        return null;
      }

      return new Token(TokenKind.Identifier)
      {
        Text = source.Text.SubstringRange(startPosition, source.Position - 1)
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