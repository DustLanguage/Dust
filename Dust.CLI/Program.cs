using System;
using System.Collections.Generic;
using System.Diagnostics;
using Dust.Compiler.Diagnostics;
using Dust.Compiler.Interpreter;
using Dust.Compiler.Lexer;
using Dust.Compiler.Parser;
using Dust.Compiler.Types;

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
        Interpreter interpreter = new Interpreter();

        List<SyntaxToken> tokens = lexer.Lex(input);

        SyntaxParseResult result = parser.Parse(tokens);

        foreach (Diagnostic diagnostic in result.Diagnostics)
        {
          Console.WriteLine($"{diagnostic.Severity}: {diagnostic.Message} at {diagnostic.Range.Start}");
        }

        DustObject value = interpreter.Interpret(result.Node);

        Console.WriteLine($"{value} ({value.TypeName})");
      }
    }
  }
}