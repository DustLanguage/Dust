﻿using System.Collections.Generic;
using Dust.Compiler.Types;

namespace Dust.Compiler.Parser.AbstractSyntaxTree
{
  public class FunctionDeclarationNode : Statement
  {
    public string Name { get; }
    public List<AccessModifier> Modifiers { get; }
    public List<FunctionParameter> Parameters { get; }
    public Node Body { get; }
    public DustType ReturnType { get; }

    public FunctionDeclarationNode(string name, List<AccessModifier> modifiers, List<FunctionParameter> parameters, CodeBlockNode body, DustType returnType, SourceRange range)
    {
      Name = name;
      Modifiers = modifiers;
      Parameters = parameters;
      Body = body;
      Range = range;
      ReturnType = returnType;
    }

    public override void Visit(IVisitor visitor)
    {
      visitor.Accept(this);

      Body.Visit(visitor);
    }

    public override void VisitChildren(IVisitor visitor)
    {
    }
  }
}