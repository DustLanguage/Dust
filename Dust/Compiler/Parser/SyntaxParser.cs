using System;
using System.Collections.Generic;
using Dust.Compiler.Diagnostics;
using Dust.Compiler.Lexer;
using Dust.Compiler.Parser.AbstractSyntaxTree;
using Dust.Compiler.Parser.Parsers;

namespace Dust.Compiler.Parser
{
  public class SyntaxParser
  {
    private readonly List<Diagnostic> diagnostics = new List<Diagnostic>();

    public List<SyntaxToken> tokens;
    private int position;

    public SyntaxToken CurrentToken => position >= tokens.Count ? SyntaxToken.Invalid : tokens[position];

    private FunctionDeclarationParser functionDeclarationParser;
    private VariableDeclarationParser variableDeclarationParser;
    private LiteralParser literalParser;

    public SyntaxParseResult Parse(List<SyntaxToken> tokens)
    {
      this.tokens = tokens;
      position = 0;

      diagnostics.Clear();

      functionDeclarationParser = new FunctionDeclarationParser(this);
      variableDeclarationParser = new VariableDeclarationParser(this);
      literalParser = new LiteralParser(this);

      if (tokens.Count == 0)
      {
        return null;
      }

      CodeBlockNode module = new CodeBlockNode();

      SourcePosition start = CurrentToken.Position;

      while (CurrentToken.Isnt(SyntaxTokenKind.EndOfFile))
      {
        if (CurrentToken.Is(SyntaxTokenKind.Invalid))
        {
          Advance();

          continue;
        }

        SyntaxNode statement = ParseDeclaration();

        module.Children.Add(statement);
      }

      module.Range = new SourceRange(start, CurrentToken.Range.End);

      return new SyntaxParseResult(module, diagnostics);
    }

    private SyntaxNode ParseDeclaration()
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

      if (startPosition == null)
      {
        startPosition = CurrentToken.Position;
      }

      if (MatchToken(SyntaxTokenKind.FnKeyword, false))
      {
        return functionDeclarationParser.Parse(startPosition);
      }

      if (MatchToken(SyntaxTokenKind.LetKeyword) || MatchToken(SyntaxTokenKind.MutKeyword))
      {
        return variableDeclarationParser.Parse(startPosition, false);
      }

      SyntaxNode node = ParseStatement();

      if (node == null)
      {
        node = variableDeclarationParser.Parse(startPosition, true);

        if (node == null)
        {
          Error(Errors.UnexpectedTokenGlobal, CurrentToken, CurrentToken.Lexeme);

          return null;
        }
      }

      return node;
    }

    public Statement ParseStatement()
    {
      Expression expression = ParseExpression();

      if (expression != null)
      {
        return new ExpressionStatement(expression);
      }

      return null;
    }

    public Expression ParseExpression()
    {
      Expression expression = literalParser.ParseLiteral();

      Advance();

      return expression;
    }

    private bool IsAccessModifier(SyntaxToken token = null)
    {
      token = token ?? CurrentToken;

      return token.IsOr(SyntaxTokenKind.PublicKeyword, SyntaxTokenKind.InternalKeyword, SyntaxTokenKind.ProtectedKeyword, SyntaxTokenKind.PrivateKeyword, SyntaxTokenKind.StaticKeyword);
    }

    public void Error(Error error, SyntaxToken token, string arg0 = null, string arg1 = null)
    {
      Error(error, token.Range, arg0, arg1);
    }

    public void Error(Error error, SourceRange range, string arg0 = null, string arg1 = null)
    {
      error.Range = range;

      if (arg0 != null || arg1 != null)
      {
        error.Format(arg0, arg1);
      }

      diagnostics.Add(error);
    }

    public void ModifierError(Error error, AccessModifier modifier, string arg0 = null, string arg1 = null)
    {
      Error(error, modifier.Token, arg0, arg1);
    }

    public bool MatchToken(SyntaxTokenKind kind, bool advance = true, int offset = 0)
    {
      if (IsAtEnd() || CurrentToken.Is(SyntaxTokenKind.EndOfFile))
      {
        return false;
      }

      if (position + offset >= tokens.Count || position + offset < 0)
      {
        return false;
      }

      bool match = tokens[position + offset].Is(kind);

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

    public SyntaxToken PeekBack(int offset = 1, bool onlyCurrentLine = true)
    {
      if (position == 0 || CurrentToken == SyntaxToken.Invalid)
      {
        return SyntaxToken.Invalid;
      }

      if (onlyCurrentLine)
      {
        return tokens[position - offset].Position.Line == CurrentToken.Position.Line ? tokens[position - offset] : SyntaxToken.Invalid;
      }

      return tokens[position - offset];
    }

    public void Revert()
    {
      position--;
    }

    public bool IsAtEnd()
    {
      return position >= tokens.Count || CurrentToken.Is(SyntaxTokenKind.EndOfFile);
    }

    public void ConsumeIf(Func<SyntaxToken, bool> condition)
    {
      if (condition(CurrentToken))
      {
        Advance();
      }
    }
  }
}