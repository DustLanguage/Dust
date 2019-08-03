﻿using System.Collections.Generic;
using Dust.Compiler.Lexer;

namespace Dust.Compiler.Parser.SyntaxTree
{
  public sealed class CodeBlockNode : SyntaxNode
  {
    public SyntaxToken OpeningBrace { get; }
    public SyntaxToken ClosingBrace { get; set; }

    public List<SyntaxNode> Children { get; }

    public CodeBlockNode(SyntaxToken openingBrace = null, SourceRange range = null)
    {
      OpeningBrace = openingBrace;
      Children = new List<SyntaxNode>();
      Range = range;
    }
  }
}