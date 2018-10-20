using Dust.Compiler.Diagnostics;
using Dust.Compiler.Lexer;
using Dust.Compiler.Parser.AbstractSyntaxTree;
using Dust.Compiler.Types;

namespace Dust.Compiler.Parser.Parsers
{
  public class VariableDeclarationParser : SyntaxParserExtension
  {
    public VariableDeclarationParser(SyntaxParser parser)
      : base(parser)
    {
    }

    public VariableDeclarationNode Parse(SourcePosition startPosition, bool hasType)
    {
      if (hasType)
      {
        Parser.Advance();
      }

      bool isMutable = Parser.PeekBack().Is(SyntaxTokenKind.MutKeyword);

      SyntaxToken typeToken = null;

      if (Parser.PeekBack().Isnt(SyntaxTokenKind.LetKeyword) && isMutable == false)
      {
        typeToken = Parser.PeekBack();
      }
      else if (Parser.CurrentToken.Is(SyntaxTokenKind.Identifier) && Parser.MatchNextToken(SyntaxTokenKind.Identifier))
      {
        typeToken = Parser.PeekBack();
      }
      else if (hasType)
      {
        typeToken = Parser.PeekBack(2);
      }

      DustType type = null;

      if (typeToken != null)
      {
        type = DustTypes.GetType(typeToken.Text);
      }

      if (Parser.MatchToken(SyntaxTokenKind.Identifier) == false)
      {
        Parser.Error(Errors.IdentifierExpected, Parser.CurrentToken, "variable declaration");

        return null;
      }

      string name = Parser.PeekBack().Text;

      Node initializer = null;

      if (Parser.MatchToken(SyntaxTokenKind.Equals))
      {
        initializer = Parser.ParseStatement();
      }

      SyntaxToken lastToken = Parser.PeekBack();

      return new VariableDeclarationNode(name, isMutable, type, initializer, new SourceRange(startPosition, new SourcePosition(lastToken.Position.Line, lastToken.Position.Column + lastToken.Text.Length)));
    }
  }
}