using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Dust.Extensions;

namespace Dust.Compiler.Lexer
{
  public class SyntaxLexer : IDisposable
  {
    private readonly StringReader source;

    public SyntaxLexer(string text)
    {
      Debug.Assert(text != null, "Text is null.");

      source = new StringReader(text);
    }

    public List<SyntaxToken> Lex()
    {
      if (string.IsNullOrWhiteSpace(source.Text))
      {
        return new List<SyntaxToken>();
      }

      List<SyntaxToken> tokens = new List<SyntaxToken>();

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

      tokens.Add(new SyntaxToken()
      {
        Kind = SyntaxTokenKind.EndOfFile,
        Position = GetSourcePosition(source.Text.Length - 1)
      });

      return tokens;
    }

    private SyntaxToken LexCharacter(char character)
    {
      SyntaxToken token = new SyntaxToken
      {
        Position = GetSourcePosition(source.Position)
      };

      switch (character)
      {
        case '(':
          token.Kind = SyntaxTokenKind.OpenParentheses;

          break;
        case ')':
          token.Kind = SyntaxTokenKind.CloseParentheses;

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

      token.Position = null;

      return token;
    }

    private SyntaxToken LexNumericLiteral()
    {
      char? character = source.Peek();

      SourcePosition position = GetSourcePosition(source.Position);

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

      return new SyntaxToken
      {
        Kind = SyntaxTokenKind.NumericLiteral,
        Position = position,
        Text = source.GetText()
      };
    }


    private SyntaxToken LexIdentifierOrKeyword()
    {
      char? character = source.Peek();

      SourcePosition position = GetSourcePosition(source.Position);

      bool invalidSymbol = false;

      source.Start();

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

      SyntaxTokenKind? keywordKind = LexKeyword(source.GetText());

      string text = source.GetText();

      source.Revert();

      return new SyntaxToken
      {
        Kind = keywordKind ?? SyntaxTokenKind.Identifier,
        Position = position,
        Text = text
      };
    }

    private SyntaxToken LexString()
    {
      char? character = source.Peek();
      bool unterminated = false;

      SourcePosition position = GetSourcePosition(source.Position - 1);

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

      return new SyntaxToken
      {
        Kind = SyntaxTokenKind.StringLiteral,
        Position = position,
        Text = source.GetText()
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
        default:
          return null;
      }
    }

    private static bool IsValidIdentiferOrKeywordCharacter(char character)
    {
      return char.IsLetterOrDigit(character) || character == '_';
    }

    private SourcePosition GetSourcePosition(int position)
    {
      Debug.Assert(position < source.Text.Length);

      string text = source.Text.SubstringRange(0, position);

      int line = text.Count(character => character == '\n');
      int column = Math.Max(position - (text.LastIndexOf('\n') == -1 ? 0 : text.LastIndexOf('\n') + 2), 0);

      return new SourcePosition(line, column);
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