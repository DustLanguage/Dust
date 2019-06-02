using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Dust.Compiler.Lexer
{
  public class SyntaxLexer
  {
    private StringReader source;

    public List<SyntaxToken> Lex(string text)
    {
      Regex.Replace(text, "(\r\n|\r|\n)", "\n");

      source = new StringReader(text);

      List<SyntaxToken> tokens = new List<SyntaxToken>();

      if (string.IsNullOrWhiteSpace(source.Text))
      {
        return tokens;
      }

      char character = source.Advance();

      while (!source.IsAtEnd())
      {
        if (char.IsWhiteSpace(character))
        {
          continue;
        }

        SyntaxToken syntaxToken = LexCharacter(character);

        character = source.Advance();

        if (syntaxToken != null)
        {
          tokens.Add(syntaxToken);
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
          token = LexStringLiteral(false);

          break;
        case '\'':
          token = LexStringLiteral(true);

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

          token = null;

          break;
      }

      if (token == null)
      {
        return new SyntaxToken
        {
          Kind = SyntaxTokenKind.Invalid,
          Range = new SourceRange(source.GetSourcePosition(start), source.SourcePosition)
        };
      }

      if (token.Position == null)
      {
        token.Position = source.SourcePosition;
      }

      if (token.Lexeme == null)
      {
        string text = source.Range(start, source.Position + 1);

        token.Lexeme = text;

        if (token.Text == null)
        {
          token.Text = text;
        }
      }

      return token;
    }

    private SyntaxToken LexNumericLiteral()
    {
      char character = source.Current;

      int startPosition = source.Position;

      bool dotFound = false;

      SyntaxTokenKind? kind = null;

      while (!source.IsAtEnd())
      {
        if (character == '.')
        {
          if (dotFound)
          {
            Console.WriteLine("invalid dot");

            return null;
          }

          dotFound = true;

          kind = SyntaxTokenKind.FloatLiteral;
        }

        if (IsNumeric(source.Peek()))
        {
          character = source.Advance();
        }
        else
        {
          break;
        }
      }

      char next = source.Peek();

      bool suffix = false;

      if (next != default)
      {
        char nextLower = char.ToLower(next);

        if (nextLower == 'd')
        {
          kind = SyntaxTokenKind.DoubleLiteral;

          suffix = true;
        }
        else if (nextLower == 'f')
        {
          kind = SyntaxTokenKind.FloatLiteral;

          suffix = true;
        }
      }

      string text = source.Range(startPosition, source.Position + 1);

      SourcePosition endPosition = source.SourcePosition;

      if (suffix)
      {
        source.Advance();
      }

      return new SyntaxToken
      {
        Kind = kind ?? SyntaxTokenKind.IntLiteral,
        Range = new SourceRange(source.GetSourcePosition(startPosition), endPosition),
        Text = text,
        Lexeme = source.Range(startPosition, source.Position + 1)
      };
    }

    private SyntaxToken LexIdentifierOrKeyword()
    {
      char character = source.Current;
      SourcePosition start = source.SourcePosition;

      while (!source.IsAtEnd() && IsValidIdentiferOrKeywordCharacter(character))
      {
        character = source.Advance();
      }

      string text = source.Range(start.Position, source.Position);

      SyntaxTokenKind? keywordKind = LexKeyword(text);

      return new SyntaxToken
      {
        Kind = keywordKind ?? SyntaxTokenKind.Identifier,
        Range = new SourceRange(start, source.SourcePosition),
        Text = text,
        Lexeme = text
      };
    }

    private SyntaxToken LexStringLiteral(bool singleQuote)
    {
      source.Advance();

      char character = source.Current;

      SourcePosition startPosition = source.SourcePosition;

      char terminator = singleQuote ? '\'' : '"';

      while (!source.IsAtEnd() && character != terminator)
      {
        character = source.Advance();
      }

      if (source.IsAtEnd())
      {
        // syntax error
        Console.WriteLine("unterminated string literal");

        return null;
      }

      string text = source.Range(startPosition, source.Position);

      SourceRange range = new SourceRange(startPosition, source.SourcePosition);

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

    private static bool IsNumeric(char character)
    {
      return char.IsDigit(character) || character == '.';
    }
  }
}