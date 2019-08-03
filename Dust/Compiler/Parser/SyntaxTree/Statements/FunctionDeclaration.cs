using System.Collections.Generic;
using Dust.Compiler.Types;

namespace Dust.Compiler.Parser.SyntaxTree
{
  public class FunctionDeclaration : Statement
  {
    public string Name { get; }
    public List<AccessModifier> Modifiers { get; }
    public List<FunctionParameter> Parameters { get; }
    public SyntaxNode Body { get; }
    public DustType ReturnType { get; }

    public FunctionDeclaration(string name, List<AccessModifier> modifiers, List<FunctionParameter> parameters, CodeBlockNode body, DustType returnType, SourceRange range)
    {
      Name = name;
      Modifiers = modifiers;
      Parameters = parameters;
      Body = body;
      Range = range;
      ReturnType = returnType;
    }
  }
}