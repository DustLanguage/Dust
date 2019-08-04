using System.Collections.Generic;
using Dust.Compiler.Diagnostics;
using Dust.Compiler.Lexer;
using Dust.Compiler.Parser.Parsers;
using Dust.Compiler.Parser.SyntaxTree;

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
    private BinaryExpressionParser binaryExpressionParser;

    public SyntaxParseResult Parse(List<SyntaxToken> tokens)
    {
      this.tokens = tokens;
      position = 0;

      diagnostics.Clear();

      if (tokens.Count == 0)
      {
        return null;
      }

      functionDeclarationParser = new FunctionDeclarationParser(this);
      variableDeclarationParser = new VariableDeclarationParser(this);
      literalParser = new LiteralParser(this);
      binaryExpressionParser = new BinaryExpressionParser(this);

      CodeBlockNode module = new CodeBlockNode();

      while (CurrentToken.Isnt(SyntaxTokenKind.EndOfFile))
      {
        if (CurrentToken.Is(SyntaxTokenKind.Invalid))
        {
          Advance();

          continue;
        }

        Statement declaration = ParseDeclaration();

        if (declaration != null)
        {
          module.Children.Add(declaration);
        }
        else
        {
          Advance();
        }
      }

      return new SyntaxParseResult(module, diagnostics);
    }

    private Statement ParseDeclaration()
    {
      if (CurrentToken.Is(SyntaxTokenKind.FnKeyword))
      {
        return functionDeclarationParser.Parse();
      }

      if (CurrentToken.IsOr(SyntaxTokenKind.LetKeyword))
      {
        return variableDeclarationParser.Parse(false);
      }

      if (CurrentToken.IsOr(SyntaxTokenKind.MutKeyword))
      {
        return variableDeclarationParser.Parse(true);
      }

      Statement statement = ParseStatement();

      if (statement == null)
      {
        Error(Errors.UnexpectedTokenGlobal(CurrentToken));

        return null;
      }

      return statement;
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
      Expression expression = literalParser.TryParse();

      /*if (SyntaxFacts.IsUnaryOperator(CurrentToken))
      {
        expression = unaryExpressionParser.Parse();
      }*/

      if (SyntaxFacts.IsBinaryOperator(CurrentToken))
      {
        return binaryExpressionParser.Parse(expression);
      }

      return expression;
    }

    public void Error(Error error)
    {
      diagnostics.Add(error);
    }

    public bool MatchToken(SyntaxTokenKind kind, int offset = 0)
    {
      if (IsAtEnd() || CurrentToken.Is(SyntaxTokenKind.EndOfFile))
      {
        return false;
      }

      if (position + offset >= tokens.Count || position + offset < 0)
      {
        return false;
      }

      if (CurrentToken.Is(kind))
      {
        Advance();

        return true;
      }

      return false;
    }

    public SyntaxToken ExpectToken(SyntaxTokenKind kind, int offset = 0)
    {
      bool match = MatchToken(kind, offset);

      if (match == false)
      {
        Error(Errors.UnexpectedToken(CurrentToken, kind));

        return SyntaxToken.Invalid;
      }

      return PeekBack();
    }

    public SyntaxToken Advance()
    {
      position++;

      return tokens[position - 1];
    }

    public SyntaxToken Peek()
    {
      return IsAtEnd() ? null : tokens[position + 1];
    }

    public SyntaxToken PeekBack(int offset = 1)
    {
      if (position == 0)
      {
        return SyntaxToken.Invalid;
      }

      return tokens[position - offset];
    }

    public void Revert()
    {
      position--;
    }

    public bool IsAtEnd()
    {
      return CurrentToken.Is(SyntaxTokenKind.EndOfFile);
    }

    public SyntaxToken ParseOptionalType()
    {
      if (MatchToken(SyntaxTokenKind.Colon))
      {
        return Advance();
      }

      return null;
    }
  }
}