using System;
using Dust.Compiler.Lexer;
using Dust.Compiler.Parser;

namespace Dust.CLI
{
  public class Program
  {
    public static void Main(string[] args)
    {
      string input;
      
      while (true)
      {
        input = Console.ReadLine();
        
        if (input == "exit")
        {
          return;
        }
        
        new SyntaxParser(new SyntaxLexer(input).Lex()).Parse();        
      }
    }
  }
}