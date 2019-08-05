using Dust.Compiler.Lexer;

namespace Dust.Compiler.Parser
{
  public class FunctionParameter
  {
    public SyntaxToken NameToken { get; }
    public SyntaxToken TypeToken { get; }
    public bool IsMutable { get; }

    public FunctionParameter(SyntaxToken nameToken, SyntaxToken typeToken, bool isMutable)
    {
      NameToken = nameToken;
      TypeToken = typeToken;
      IsMutable = isMutable;
    }
  }
}