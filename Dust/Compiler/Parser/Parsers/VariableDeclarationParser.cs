using Dust.Compiler.Diagnostics;
using Dust.Compiler.Lexer;
using Dust.Compiler.Parser.SyntaxTree;
using Dust.Compiler.Types;

namespace Dust.Compiler.Parser.Parsers
{
  public class VariableDeclarationParser : SyntaxParserExtension
  {
    public VariableDeclarationParser(SyntaxParser parser)
      : base(parser)
    {
    }

    public VariableDeclaration Parse(SourcePosition startPosition, bool hasType)
    {
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
        typeToken = Parser.PeekBack();
      }

      DustType type = null;

      if (typeToken != null)
      {
        type = DustTypes.GetType(typeToken.Text);

        if (type == null)
        {
          Parser.Error(Errors.UnknownType, typeToken, typeToken.Text);
        }
      }

      if (Parser.MatchToken(SyntaxTokenKind.Identifier) == false)
      {
        Parser.Error(Errors.IdentifierExpected, Parser.CurrentToken, "variable declaration");

        return null;
      }

      string name = Parser.PeekBack().Text;

      SyntaxNode initializer = null;

      if (Parser.MatchToken(SyntaxTokenKind.Equals))
      {
        initializer = Parser.ParseStatement();
      }

      return new VariableDeclaration(name, isMutable, type, initializer, new SourceRange(startPosition, Parser.CurrentToken.Range.End));
    }
  }
}