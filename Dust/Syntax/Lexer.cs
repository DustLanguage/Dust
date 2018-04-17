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
      if (string.IsNullOrWhiteSpace(source.Text))
      {
        return new List<Token>();
      }

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
        case '"':
        case '\'':
          return LexString();
        default:
          if (char.IsDigit(character))
          {
            return LexNumericLiteral();
          }

          if (char.IsLetter(character) || character == '_')
          {
            return LexIdentifierOrKeyword();
          }

          return null;
      }
    }

    private Token LexNumericLiteral()
    {
      char? character = source.Peek();

      bool dotFound = false;
      bool invalidDot = false;

      source.Start();

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
        // This might need to be here
        // source.Revert();

        // Syntax error
        Console.WriteLine("invalid dot");
        
        return null;
      }

      return new Token(TokenKind.NumericLiteral)
      {
        Text = source.GetText()
      };
    }


    private Token LexIdentifierOrKeyword()
    {
      char? character = source.Peek();

      bool invalidSymbol = false;

      source.Start();

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
        // Syntax error
        Console.WriteLine("Invalid symbol in identifier.");

        return null;
      }
      
      TokenKind? keywordKind = LexKeyword(source.GetText());

      return new Token(keywordKind ?? TokenKind.Identifier)
      {
        Text = source.GetText()
      };
    }

    private Token LexString()
    {
      char? character = source.Peek();

      bool unterminated = false;

      source.Start(source.Position + 1);

      while (character != null && IsStringTerminator(character.Value) == false)
      {
        if (source.IsAtEnd())
        {
          unterminated = true;

          break;
        }

        character = source.Advance();
      }

      if (unterminated)
      {
        // Syntax error
        Console.WriteLine("unterminated string");

        return null;
      }

      return new Token(TokenKind.StringLiteral)
      {
        Text = source.GetText()
      };
    }

    private TokenKind? LexKeyword(string text)
    {
      switch (text)
      {
        case "let":
          return TokenKind.LetKeyword;
        case "fn":
          return TokenKind.FnKeyword;
        case "mut":
          return TokenKind.MutKeyword;
        case "return":
          return TokenKind.ReturnKeyword;
        case "typeof":
          return TokenKind.TypeOfKeyword;
        case "true":
          return TokenKind.TrueKeyword;
        case "false":
          return TokenKind.FalseKeyword;
        case "if":
          return TokenKind.IfKeyword;
        case "else":
          return TokenKind.ElseKeyword;
        case "elif":
          return TokenKind.ElifKeyword;
        case "null":
          return TokenKind.NullKeyword;
        default:
          return null;
      }
    }

    private static bool IsValidIdentiferOrKeywordCharacter(char character)
    {
      return char.IsLetterOrDigit(character) || character == '_';
    }

    private static bool IsStringTerminator(char character)
    {
      return character == '\'' || character == '"';
    }

    public void Dispose()
    {
      source.Dispose();
    }
  }
}