using System.Collections.Generic;
using Dust.Compiler.Diagnostics;
using Dust.Compiler.Lexer;
using Dust.Compiler.parser.parsers;
using Dust.Compiler.Parser.AbstractSyntaxTree;
using Dust.Compiler.Parser.Parsers;

namespace Dust.Compiler.Parser
{
  public class SyntaxParser
  {
    public List<Diagnostic> Diagnostics { get; } = new List<Diagnostic>();

    public List<SyntaxToken> tokens;
    private int position;

    public SyntaxToken CurrentToken => tokens[position];

    private readonly FunctionDeclarationParser functionDeclarationParser = new FunctionDeclarationParser();
    private readonly PropertyDeclarationParser propertyDeclarationParser = new PropertyDeclarationParser();

    public Node Parse(List<SyntaxToken> tokens)
    {
      this.tokens = tokens;
      position = 0;

      if (tokens.Count == 0)
      {
        return null;
      }

      CodeBlockNode module = new CodeBlockNode();

      SourcePosition start = CurrentToken.Position;

      while (position < tokens.Count - 1)
      {
        Node statement = ParseDeclaration();

        if (statement == null)
        {
          break;
        }

        module.Children.Add(statement);
      }

      module.Range = new SourceRange(start, CurrentToken.Position);

      return module;
    }

    private Node ParseDeclaration()
    {
      SourcePosition startPosition = null;

      while (IsAccessModifier())
      {
        if (startPosition == null)
        {
          startPosition = CurrentToken.Position;
        }

        Advance();
      }

      if (MatchToken(SyntaxTokenKind.FnKeyword, false))
      {
        return functionDeclarationParser.Parse(this, startPosition);
      }

      if (MatchToken(SyntaxTokenKind.LetKeyword))
      {
        return propertyDeclarationParser.Parse(this, startPosition);
      }

      Node node = ParseStatement();

      if (node == null)
      {
        Error(Errors.UnexpectedTokenGlobal, CurrentToken.Range, CurrentToken.Lexeme);

        return null;
      }

      return node;
    }

    public Node ParseStatement()
    {
      return null;
    }

    private bool IsAccessModifier(SyntaxToken token = null)
    {
      SyntaxTokenKind kind = token?.Kind ?? CurrentToken.Kind;

      return kind == SyntaxTokenKind.PublicKeyword || kind == SyntaxTokenKind.InternalKeyword || kind == SyntaxTokenKind.ProtectedKeyword || kind == SyntaxTokenKind.PrivateKeyword || kind == SyntaxTokenKind.StaticKeyword;
    }

    public void Error(Error error, SourceRange range, string arg0 = null, string arg1 = null)
    {
      error.Range = range;

      if (arg0 != null || arg1 != null)
      {
        error.Format(arg0, arg1);
      }

      Diagnostics.Add(error);
    }

    public void ModifierError(Error error, AccessModifier modifier, string arg0 = null, string arg1 = null)
    {
      Error(error, modifier.Token.Range, arg0, arg1);
    }

    public bool MatchToken(SyntaxTokenKind kind, bool advance = true, int offset = 0)
    {
      if (IsAtEnd() || CurrentToken.Kind == SyntaxTokenKind.EndOfFile) return false;

      bool match = tokens[position + offset].Kind == kind;

      if (match && advance)
      {
        position++;
      }

      return match;
    }

    public bool MatchNextToken(SyntaxTokenKind kind, bool advance = true)
    {
      return MatchToken(kind, advance, 1);
    }

    public void Advance()
    {
      position++;
    }

    public SyntaxToken Peek()
    {
      return IsAtEnd() ? null : tokens[position + 1];
    }

    public SyntaxToken PeekBack()
    {
      return position == 0 ? null : tokens[position - 1];
    }

    public void Revert()
    {
      position--;
    }

    public bool IsAtEnd()
    {
      return position >= tokens.Count || CurrentToken.Kind == SyntaxTokenKind.EndOfFile;
    }
  }
}