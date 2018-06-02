namespace Dust.Compiler.Diagnostics
{
  public class Diagnostic
  {
    public string Message { get; private set; }
    public DiagnosticSeverity Severity { get; }
    public SourceRange Range { get; set; }

    public Diagnostic(string message, DiagnosticSeverity severity)
    {
      Message = message;
      Severity = severity;
    }
    
    public void Format(string arg0, string arg1)
    {
        Message = arg1 != null ? string.Format(Message, arg0, arg1) : string.Format(Message, arg0);
    }
  }
}