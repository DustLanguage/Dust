using System;
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
        string input = Console.ReadLine();

        if (input == "exit")
        {
          return;
        }

        SyntaxLexer lexer = new SyntaxLexer();
        SyntaxParser parser = new SyntaxParser();

        parser.Parse(lexer.Lex(input));

        foreach (Diagnostic diagnostic in parser.Diagnostics)
        {
          Console.WriteLine($"{diagnostic.Severity}: {diagnostic.Message} at {diagnostic.Range.Start}");
        }
      }
    }
  }
}