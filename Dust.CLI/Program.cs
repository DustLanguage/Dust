using System;
using Dust.Compiler.Diagnostics;
using Dust.Compiler.Lexer;
using Dust.Compiler.Parser;

namespace Dust.CLI
{
  public class Program
  {
    public static void Main(string[] args)
    {
      while (true)
      {
        string input = Console.ReadLine();

        if (input == "exit")
        {
          return;
        }

        SyntaxParser parser = new SyntaxParser(new SyntaxLexer(input).Lex());

        parser.Parse();

        foreach (Diagnostic diagnostic in parser.Diagnostics)
        {
          Console.WriteLine($"{diagnostic.Severity}: {diagnostic.Message} at {diagnostic.Range.Start} ({diagnostic.Code})");
        }
      }
    }
  }
}