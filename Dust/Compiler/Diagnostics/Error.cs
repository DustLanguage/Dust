namespace Dust.Compiler.Diagnostics
{
  public class Error : Diagnostic
  {
    public Error(string message, SourceRange range) : base(message, range, DiagnosticSeverity.Error)
    {
    }
  }
}