using System;
using System.Collections.Generic;
using Dust.Compiler.Diagnostics;
using Dust.Compiler.Lexer;
using Dust.Compiler.Parser;

namespace Dust.CLI
{
  public static class Program
  {
    public static void Main()
    {
      while (true)
      {
        Console.Write("> ");

        string input = Console.ReadLine();

        if (input == "")
        {
          continue;
        }

        if (input == "exit")
        {
          return;
        }

        SyntaxLexer lexer = new SyntaxLexer();
        SyntaxParser parser = new SyntaxParser();

        List<SyntaxToken> tokens = lexer.Lex(input);

        foreach (SyntaxToken token in tokens)
        {
          Console.WriteLine($"{token.Kind}: '{token.Text}' {token.Lexeme}");
        }

        SyntaxParseResult result = parser.Parse(tokens);

        foreach (Diagnostic diagnostic in result.Diagnostics)
        {
          Console.WriteLine($"{diagnostic.Severity}: {diagnostic.Message} at {diagnostic.Range.Start}");
        }
      }
    }
  }
}