using System.Collections.Generic;
using Dust.Compiler.Diagnostics;
using Dust.Compiler.Lexer;
using Dust.Compiler.Parser.SyntaxTree;

namespace Dust.Compiler.Parser.Parsers
{
  public class FunctionDeclarationParser : SyntaxParserExtension
  {
    public FunctionDeclarationParser(SyntaxParser parser)
      : base(parser)
    {
    }

    public FunctionDeclaration Parse()
    {
      SyntaxToken fnToken = Parser.ExpectToken(SyntaxTokenKind.FnKeyword);
      SyntaxToken nameToken = Parser.ExpectToken(SyntaxTokenKind.Identifier);
      SyntaxToken closeParenthesisToken = null;

      List<FunctionParameter> parameters = new List<FunctionParameter>();

      if (Parser.MatchToken(SyntaxTokenKind.OpenParenthesis))
      {
        if (Parser.MatchToken(SyntaxTokenKind.CloseParenthesis) == false)
        {
          // TODO: This probably shouldn't be an infinite loop
          while (true)
          {
            bool isMutable = Parser.MatchToken(SyntaxTokenKind.MutKeyword);

            SyntaxToken parameterNameToken = Parser.ExpectToken(SyntaxTokenKind.Identifier);
            SyntaxToken parameterTypeToken = Parser.ParseOptionalType();

            parameters.Add(new FunctionParameter(parameterNameToken, parameterTypeToken, isMutable));

            if (Parser.MatchToken(SyntaxTokenKind.Comma) == false)
            {
              closeParenthesisToken = Parser.ExpectToken(SyntaxTokenKind.CloseParenthesis);

              break;
            }
          }
        }
        else
        {
          closeParenthesisToken = Parser.PeekBack();
        }
      }

      SyntaxToken returnTypeToken = Parser.ParseOptionalType();

      CodeBlockNode bodyNode = null;

      if (Parser.MatchToken(SyntaxTokenKind.OpenBrace))
      {
        bodyNode = new CodeBlockNode(Parser.PeekBack());

        while (Parser.MatchToken(SyntaxTokenKind.CloseBrace) == false)
        {
          if (Parser.IsAtEnd())
          {
            Parser.Error(Errors.ExpectedToken(Parser.CurrentToken, SyntaxTokenKind.CloseBrace));
          }

          Statement statement = Parser.ParseStatement();

          if (statement == null)
          {
            break;
          }

          bodyNode.Children.Add(statement);
        }

        bodyNode.ClosingBrace = Parser.PeekBack();
      }

      return new FunctionDeclaration(fnToken, nameToken, parameters, closeParenthesisToken, bodyNode, returnTypeToken);
    }
  }
}