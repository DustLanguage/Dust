using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Dust.Compiler.Lexer
{
  public class SyntaxLexer
  {
    private StringReader source;

    public List<SyntaxToken> Lex(string text)
    {
      Debug.Assert(text != null);

      source = new StringReader(text);

      List<SyntaxToken> tokens = new List<SyntaxToken>();

      if (string.IsNullOrWhiteSpace(source.Text))
      {
        return tokens;
      }

      char? character = source.Text[0];

      while (true)
      {
        if (source.Position + 1 <= source.Text.Length)
        {
          SyntaxToken syntaxToken = LexCharacter(character.Value);

          character = source.Advance();

          if (syntaxToken != null)
          {
            tokens.Add(syntaxToken);
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

      tokens.Add(new SyntaxToken
      {
        Kind = SyntaxTokenKind.EndOfFile,
        Position = source.GetSourcePosition(source.Text.Length - 1)
      });

      return tokens;
    }

    private SyntaxToken LexCharacter(char character)
    {
      int start = source.Position;

      SyntaxToken token = new SyntaxToken
      {
        Position = source.GetSourcePosition(start)
      };

      switch (character)
      {
        case '(':
          token.Kind = SyntaxTokenKind.OpenParenthesis;

          break;
        case ')':
          token.Kind = SyntaxTokenKind.CloseParenthesis;

          break;
        case '{':
          token.Kind = SyntaxTokenKind.OpenBrace;

          break;
        case '}':
          token.Kind = SyntaxTokenKind.CloseBrace;

          break;
        case '[':
          token.Kind = SyntaxTokenKind.OpenBracket;

          break;
        case ']':
          token.Kind = SyntaxTokenKind.CloseBracket;

          break;
        case '.':
          token.Kind = SyntaxTokenKind.Dot;

          break;
        case ',':
          token.Kind = SyntaxTokenKind.Comma;

          break;
        case '=':
          token.Kind = SyntaxTokenKind.Equals;

          break;
        case '+':
          if (source.Peek() == '=')
          {
            source.Advance();

            token.Kind = SyntaxTokenKind.PlusEquals;

            break;
          }

          if (source.Peek() == '+')
          {
            source.Advance();

            token.Kind = SyntaxTokenKind.PlusPlus;

            break;
          }

          token.Kind = SyntaxTokenKind.Plus;

          break;
        case '-':
          if (source.Peek() == '=')
          {
            source.Advance();

            token.Kind = SyntaxTokenKind.MinusEquals;

            break;
          }

          if (source.Peek() == '-')
          {
            source.Advance();

            token.Kind = SyntaxTokenKind.MinusMinus;

            break;
          }

          token.Kind = SyntaxTokenKind.Minus;

          break;
        case '*':
          if (source.Peek() == '=')
          {
            source.Advance();

            token.Kind = SyntaxTokenKind.AsteriskEquals;

            break;
          }

          if (source.Peek() == '*')
          {
            source.Advance();

            token.Kind = SyntaxTokenKind.AsteriskAsterisk;

            break;
          }

          token.Kind = SyntaxTokenKind.Asterisk;

          break;
        case '/':
          if (source.Peek() == '=')
          {
            source.Advance();

            token.Kind = SyntaxTokenKind.SlashEquals;

            break;
          }

          if (source.Peek() == '/')
          {
            source.Advance();

            token.Kind = SyntaxTokenKind.SlashSlash;

            break;
          }

          token.Kind = SyntaxTokenKind.Slash;

          break;
        case '"':
        case '\'':
          token = LexStringLiteral();

          break;
        case '\n':
          token.Kind = SyntaxTokenKind.EndOfLine;

          break;
        default:
          if (char.IsDigit(character))
          {
            token = LexNumericLiteral();

            break;
          }

          if (char.IsLetter(character) || character == '_')
          {
            token = LexIdentifierOrKeyword();

            break;
          }

          return null;
      }

      if (token.Position == null)
      {
        token.Position = source.SourcePosition;
      }

      if (token.Lexeme == null)
      {
        token.Lexeme = source.Range(start, source.Position + 1);
      }

      return token;
    }

    private SyntaxToken LexNumericLiteral()
    {
      char? character = source.Advance();

      int startPosition = source.Position - 1;

      bool dotFound = false;
      bool invalidDot = false;

      SyntaxTokenKind? kind = null;

      while (character != null && (char.IsDigit(character.Value) || character.Value == '.'))
      {
        if (character.Value == '.')
        {
          if (dotFound)
          {
            invalidDot = true;
          }

          dotFound = true;

          kind = SyntaxTokenKind.FloatLiteral;
        }

        character = source.Advance();
      }

      if (invalidDot)
      {
        // Syntax error
        Console.WriteLine("invalid dot");

        return null;
      }

      if (character != null && (character.Value == 'd' || character.Value == 'D'))
      {
        kind = SyntaxTokenKind.DoubleLiteral;
      }

      string text = source.Range(startPosition, source.Position);

      return new SyntaxToken
      {
        Kind = kind ?? SyntaxTokenKind.IntLiteral,
        Range = new SourceRange(source.GetSourcePosition(startPosition), source.SourcePosition),
        Text = text,
        Lexeme = text
      };
    }

    private SyntaxToken LexIdentifierOrKeyword()
    {
      char? character = source.Current;
      SourcePosition start = source.SourcePosition;

      bool invalidSymbol = false;

      while (true)
      {
        if (character != null && IsValidIdentiferOrKeywordCharacter(character.Value))
        {
          character = source.Advance();
        }
        else if (character == ' ' || character == null || IsValidIdentiferOrKeywordCharacter(character.Value) == false)
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

      string text = source.Range(start.Position, source.Position);

      SyntaxTokenKind? keywordKind = LexKeyword(text);

      source.Revert();

      return new SyntaxToken
      {
        Kind = keywordKind ?? SyntaxTokenKind.Identifier,
        Range = new SourceRange(start, source.SourcePosition),
        Text = text
      };
    }

    private SyntaxToken LexStringLiteral()
    {
      char? character = source.Peek();
      bool unterminated = false;

      SourcePosition startPosition = source.SourcePosition;

      source.Advance();

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
        // syntax error
        Console.WriteLine("unterminated string literal");

        return null;
      }

      string text = source.Range(startPosition + 1, source.Position);
      SourcePosition endPosition = source.SourcePosition + 1;

      if (text == "")
      {
        endPosition = source.SourcePosition;
      }

      SourceRange range = new SourceRange(startPosition, endPosition);

      return new SyntaxToken
      {
        Kind = SyntaxTokenKind.StringLiteral,
        Position = startPosition,
        Range = range,
        Text = text
      };
    }

    private static SyntaxTokenKind? LexKeyword(string text)
    {
      switch (text)
      {
        case "let":
          return SyntaxTokenKind.LetKeyword;
        case "fn":
          return SyntaxTokenKind.FnKeyword;
        case "mut":
          return SyntaxTokenKind.MutKeyword;
        case "return":
          return SyntaxTokenKind.ReturnKeyword;
        case "typeof":
          return SyntaxTokenKind.TypeOfKeyword;
        case "true":
          return SyntaxTokenKind.TrueKeyword;
        case "false":
          return SyntaxTokenKind.FalseKeyword;
        case "if":
          return SyntaxTokenKind.IfKeyword;
        case "else":
          return SyntaxTokenKind.ElseKeyword;
        case "elif":
          return SyntaxTokenKind.ElifKeyword;
        case "null":
          return SyntaxTokenKind.NullKeyword;
        case "public":
          return SyntaxTokenKind.PublicKeyword;
        case "internal":
          return SyntaxTokenKind.InternalKeyword;
        case "protected":
          return SyntaxTokenKind.ProtectedKeyword;
        case "private":
          return SyntaxTokenKind.PrivateKeyword;
        case "static":
          return SyntaxTokenKind.StaticKeyword;
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
  }
}