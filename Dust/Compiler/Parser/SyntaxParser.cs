using System;
using System.Collections.Generic;
using Dust.Compiler.Lexer;
using Dust.Compiler.Parser.AbstractSyntaxTree;

namespace Dust.Compiler.Parser
{
  public class SyntaxParser
  {
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

    private Node ParseStatement()
    {
      if (MatchNextToken(SyntaxTokenKind.FnKeyword))
      {
        return ParseFn();
      }

      if (MatchToken(SyntaxTokenKind.LetKeyword))
      {
        return ParseLet();
      }

      return null;
    }

    private Node ParseFn()
    {
      if (PeekBack().Kind != SyntaxTokenKind.LetKeyword)
      {
        // Syntax error

        Console.WriteLine("error");
      }

      SourcePosition position = PeekBack().Position;

      List<FunctionModifier> modifiers = new List<FunctionModifier>();

      while (true)
      {
        FunctionModifier modifier = FunctionModifier.Parse(CurrentToken.Kind);

        if (modifier == null)
        {
          break;
        }

        modifiers.Add(modifier);
      }

      Advance();

      if (MatchToken(SyntaxTokenKind.Identifier, false) == false)
      {
        Console.WriteLine("error");
      }

      string name = CurrentToken.Text;

      Advance();

      if (MatchToken(SyntaxTokenKind.OpenParentheses) == false)
      {
        Console.WriteLine("error");
      }

      if (IsAtEnd())
      {
        Revert();

        return null;
      }

      SourcePosition bodyStartPosition = CurrentToken.Position;

      List<FunctionParameter> parameters = new List<FunctionParameter>();

      if (MatchToken(SyntaxTokenKind.CloseParentheses) == false)
      {
        while (MatchToken(SyntaxTokenKind.CloseParentheses))
        {
          if (IsAtEnd())
          {
            Console.WriteLine("error");
          }

          bool isMutable = MatchToken(SyntaxTokenKind.MutKeyword);

          if (MatchToken(SyntaxTokenKind.Identifier))
          {
            parameters.Add(new FunctionParameter(CurrentToken.Text, null, isMutable));
          }
        }
      }

      if (MatchToken(SyntaxTokenKind.OpenBrace) == false)
      {
        Console.WriteLine("error");
      }

      CodeBlockNode bodyNode = new CodeBlockNode();

      while (MatchToken(SyntaxTokenKind.CloseBrace, false) == false)
      {
        if (IsAtEnd())
        {
          Console.WriteLine("error");

          break;
        }

        bodyNode.Children.Add(ParseStatement());
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