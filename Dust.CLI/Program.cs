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
      new Lexer(Console.ReadLine()).Lex();
    }
  }
}