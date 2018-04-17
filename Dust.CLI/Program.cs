using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Dust.Syntax;
using LLVMSharp;

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
        
        new Lexer(input).Lex();        
      }
    }
  }
}