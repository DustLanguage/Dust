using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Dust.Compiler.Diagnostics
{
  public class Diagnostic
  {
    public string Message { get; private set; }
    public DiagnosticSeverity Severity { get; }
    public SourceRange Range { get; set; }

    protected Diagnostic(string message, DiagnosticSeverity severity)
    {
      Message = message;
      Severity = severity;
    }

    public void Format(string arg0, string arg1)
    {
      int formattablePlaceholders = Regex.Matches(Message, @"(?<!\{)\{([0-9]+).*?\}(?!})").Count;

      Debug.Assert(formattablePlaceholders <= 2);

      if (formattablePlaceholders > 1)
      {
        Debug.Assert(arg0 != null && arg1 != null);
      }
      else if (formattablePlaceholders > 0)
      {
        Debug.Assert(formattablePlaceholders > 0 && arg0 != null);
      }

      Message = arg1 != null ? string.Format(Message, arg0, arg1) : string.Format(Message, arg0);
    }
  }
}