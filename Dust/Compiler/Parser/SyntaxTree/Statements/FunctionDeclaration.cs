using System.Collections.Generic;
using Dust.Compiler.Lexer;

namespace Dust.Compiler.Parser.SyntaxTree
{
  public class FunctionDeclaration : Statement
  {
    public SyntaxToken FnToken { get; }
    public SyntaxToken NameToken { get; }
    public List<FunctionParameter> Parameters { get; }
    public SyntaxToken ClosingParenthesis { get; }
    public SyntaxToken ReturnTypeToken { get; }
    public CodeBlockNode Body { get; }

    public FunctionDeclaration(SyntaxToken fnToken, SyntaxToken nameToken, List<FunctionParameter> parameters, SyntaxToken closingParenthesis, CodeBlockNode body, SyntaxToken returnTypeToken)
    {
      FnToken = fnToken;
      NameToken = nameToken;
      Parameters = parameters;
      ClosingParenthesis = closingParenthesis;
      Body = body;
      ReturnTypeToken = returnTypeToken;
    }
  }
}