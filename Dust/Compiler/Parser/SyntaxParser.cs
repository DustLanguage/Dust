using System;
using System.Collections.Generic;
using System.Diagnostics;
using Dust.Compiler.Diagnostics;
using Dust.Compiler.Lexer;
using Dust.Compiler.Parser.AbstractSyntaxTree;

namespace Dust.Compiler.Parser
{
  public class SyntaxParser
  {
    public List<Diagnostic> Diagnostics { get; } = new List<Diagnostic>();

    private readonly List<SyntaxToken> tokens;
    private int position;

    private SyntaxToken CurrentToken => tokens[position];

    public SyntaxParser(List<SyntaxToken> tokens)
    {
      this.tokens = tokens;
      position = 0;
    }

    public Node Parse()
    {
      if (tokens.Count == 0)
      {
        return null;
      }

      CodeBlockNode module = new CodeBlockNode();

      SourcePosition start = CurrentToken.Position;

      while (position < tokens.Count - 1)
      {
        Node statement = ParseStatement();

        if (statement == null)
        {
          break;
        }

        module.Children.Add(statement);
      }

      module.Range = new SourceRange(start, CurrentToken.Position);

      return module;
    }

    public Node ParseStatement()
    {
      if (MatchToken(SyntaxTokenKind.LetKeyword))
      {
        return ParseLet();
      }
      
      if (MatchToken(SyntaxTokenKind.FnKeyword, false) || MatchNextToken(SyntaxTokenKind.FnKeyword, false) || IsFunctionStartToken())
      {
        return ParseFn();
      }

      

      Error(Errors.UnexpectedToken, CurrentToken.Range);

      return null;
    }

    private Node ParseFn()
    {
      SourcePosition position = CurrentToken.Position;

      while (IsFunctionStartToken())
      {
        Advance();
      }

      if (MatchToken(SyntaxTokenKind.LetKeyword) == false)
      {
        Error(Errors.LetExpected, CurrentToken.Range);
      }

      List<FunctionModifier> modifiers = new List<FunctionModifier>();

      for (int i = tokens.IndexOf(CurrentToken); i >= 0; i--)
      {
        FunctionModifier modifier = FunctionModifier.Parse(tokens[i].Kind);

        if (modifier == null)
        {
          continue;
        }

        modifiers.Add(modifier);
      }

      if (MatchToken(SyntaxTokenKind.Identifier, false) == false)
      {
        Error(Errors.IdentifierExpected, CurrentToken.Range);
      }

      string name = CurrentToken.Text;

      Advance();

      if (MatchToken(SyntaxTokenKind.OpenParenthesis) == false)
      {
        Error(Errors.OpenParenthesisExpected, CurrentToken.Range);
      }

      if (IsAtEnd())
      {
        Revert();

        return null;
      }

      SourcePosition bodyStartPosition = CurrentToken.Position;

      List<FunctionParameter> parameters = new List<FunctionParameter>();

      if (MatchToken(SyntaxTokenKind.CloseParenthesis) == false)
      {
        while (MatchToken(SyntaxTokenKind.CloseParenthesis) == false)
        {
          bool isMutable = MatchToken(SyntaxTokenKind.MutKeyword);

          if (MatchToken(SyntaxTokenKind.Identifier))
          {
            parameters.Add(new FunctionParameter(CurrentToken.Text, null, isMutable));
          }
          else
          {
            Error(Errors.CloseParenthesisExpected, CurrentToken.Range);

            break;
          }
        }
      }

      if (MatchToken(SyntaxTokenKind.OpenBrace) == false)
      {
        Error(Errors.OpenBraceExpected, new SourceRange(CurrentToken.Position, CurrentToken.Position + 1));
      }

      CodeBlockNode bodyNode = new CodeBlockNode();

      SyntaxToken closeBrace = CurrentToken;

      while (MatchToken(SyntaxTokenKind.CloseBrace) == false)
      {
        if (IsAtEnd())
        {
          Error(Errors.CloseBraceExpected, new SourceRange(closeBrace.Position, closeBrace.Position + 1));

          break;
        }

        Node statement = ParseStatement();

        if (statement == null)
        {
          break;
        }

        bodyNode.Children.Add(statement);
      }

      bodyNode.Range = new SourceRange(bodyStartPosition, CurrentToken.Position);

      return new FunctionDeclarationNode(name, modifiers, parameters, bodyNode, new SourceRange(position, CurrentToken.Position));
    }

    private PropertyDeclarationNode ParseLet()
    {
      SourcePosition startPosition = PeekBack().Position;

      bool isMutable = MatchToken(SyntaxTokenKind.MutKeyword);

      if (CurrentToken.Kind != SyntaxTokenKind.Identifier)
      {
        // Syntax error
        Console.WriteLine("error");
      }

      PropertyDeclarationNode node = new PropertyDeclarationNode(CurrentToken.Text, isMutable)
      {
        Range = new SourceRange(startPosition, new SourcePosition(CurrentToken.Position.Line, CurrentToken.Position.Column + CurrentToken.Text.Length))
      };

      Advance();

      return node;
    }

    private bool IsFunctionStartToken(SyntaxToken token = null)
    {
      SyntaxTokenKind kind = token?.Kind ?? CurrentToken.Kind;

      return kind == SyntaxTokenKind.PublicKeyword || kind == SyntaxTokenKind.InternalKeyword || kind == SyntaxTokenKind.ProtectedKeyword || kind == SyntaxTokenKind.PrivateKeyword || kind == SyntaxTokenKind.StaticKeyword;
    }

    private void Error(Diagnostic error, SourceRange range)
    {
      Debug.Assert(error.Severity == DiagnosticSeverity.Error);

      error.Range = range;

      Diagnostics.Add(error);
    }

    private bool MatchToken(SyntaxTokenKind kind, bool advance = true, int offset = 0)
    {
      if (IsAtEnd() || CurrentToken.Kind == SyntaxTokenKind.EndOfFile) return false;

      bool match = tokens[position + offset].Kind == kind;

      if (match && advance)
      {
        position++;
      }

      return match;
    }

    private bool MatchNextToken(SyntaxTokenKind kind, bool advance = true)
    {
      return MatchToken(kind, advance, 1);
    }

    private void Advance()
    {
      position++;
    }

    private SyntaxToken Peek()
    {
      return IsAtEnd() ? null : tokens[position + 1];
    }

    private SyntaxToken PeekBack()
    {
      return position == 0 ? null : tokens[position - 1];
    }

    private void Revert()
    {
      position--;
    }

    private bool IsAtEnd()
    {
      return position >= tokens.Count || CurrentToken.Kind == SyntaxTokenKind.EndOfFile;
    }
  }
}