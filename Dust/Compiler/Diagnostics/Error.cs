namespace Dust.Compiler.Diagnostics
{
  public class Error : Diagnostic
  {
    public Error(string message) : base(message, DiagnosticSeverity.Error)
    {
    }
  }
}