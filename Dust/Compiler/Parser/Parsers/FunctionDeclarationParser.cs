using System.Collections.Generic;
using System.Linq;
using Dust.Compiler.Diagnostics;
using Dust.Compiler.Lexer;
using Dust.Compiler.Parser;
using Dust.Compiler.Parser.AbstractSyntaxTree;
using Dust.Compiler.Parser.Parsers;
using Dust.Extensions;

namespace Dust.Compiler.parser.parsers
{
  public class FunctionDeclarationParser : ISyntaxParserExtension
  {
    public Node Parse(SyntaxParser parser, SourcePosition startPosition)
    {
      if (parser.MatchToken(SyntaxTokenKind.FnKeyword) == false)
      {
        parser.Error(Errors.FnExpected, parser.CurrentToken.Range);
      }

      List<AccessModifier> modifiers = new List<AccessModifier>();

      bool modifierSeen = false;

      for (int i = parser.tokens.IndexOf(parser.CurrentToken); i >= 0; i--)
      {
        AccessModifierKind? kind = AccessModifier.ParseKind(parser.tokens[i].Kind);

        if (kind == null)
        {
          if (modifierSeen)
          {
            break;
          }

          continue;
        }

        AccessModifier modifier = new AccessModifier(parser.tokens[i], kind.Value);

        modifiers.Add(modifier);

        modifierSeen = true;
      }

      if (parser.MatchToken(SyntaxTokenKind.Identifier, false) == false)
      {
        parser.Error(Errors.IdentifierExpected, parser.CurrentToken.Range, "function declaration");
      }

      ValidateFunctionModifiers(parser, modifiers, parser.PeekBack().Range);

      string name = parser.CurrentToken.Text;

      parser.Advance();

      if (parser.MatchToken(SyntaxTokenKind.OpenParenthesis) == false)
      {
        parser.Error(Errors.OpenParenthesisExpected, parser.CurrentToken.Range);
      }

      if (parser.IsAtEnd())
      {
        parser.Revert();

        return null;
      }

      List<FunctionParameter> parameters = new List<FunctionParameter>();

      bool comma = false;

      if (parser.MatchToken(SyntaxTokenKind.CloseParenthesis) == false)
      {
        while (parser.MatchToken(SyntaxTokenKind.CloseParenthesis) == false)
        {
          if (parser.MatchToken(SyntaxTokenKind.Comma))
          {
            comma = true;

            continue;
          }

          bool isMutable = parser.MatchToken(SyntaxTokenKind.MutKeyword);

          if (parser.MatchToken(SyntaxTokenKind.Identifier))
          {
            // Type

            if (parser.MatchToken(SyntaxTokenKind.Identifier))
            {
              // Identifier

              parameters.Add(new FunctionParameter(parser.PeekBack().Text, null, isMutable));

              comma = false;
            }
          }
          else
          {
            parser.Error(Errors.CloseParenthesisExpected, parser.CurrentToken.Range);

            break;
          }
        }
      }

      if (comma)
      {
        parser.Error(Errors.ExpectedAfter, parser.CurrentToken.Range, "parameter", "comma");
      }

      SourcePosition bodyStartPosition = parser.CurrentToken.Position;
      
      if (parser.MatchToken(SyntaxTokenKind.OpenBrace) == false)
      {
        parser.Error(Errors.OpenBraceExpected, new SourceRange(parser.CurrentToken.Position, parser.CurrentToken.Position + 1));
      }

      CodeBlockNode bodyNode = new CodeBlockNode();

      SyntaxToken closeBrace = parser.CurrentToken;

      while (parser.MatchToken(SyntaxTokenKind.CloseBrace) == false)
      {
        if (parser.IsAtEnd())
        {
          parser.Error(Errors.CloseBraceExpected, new SourceRange(closeBrace.Position, closeBrace.Position + 1));

          break;
        }

        Node statement = parser.ParseStatement();

        if (statement == null)
        {
          break;
        }

        bodyNode.Children.Add(statement);
      }

      bodyNode.Range = new SourceRange(bodyStartPosition, parser.CurrentToken.Position);

      return new FunctionDeclarationNode(name, modifiers, parameters, bodyNode, new SourceRange(startPosition, parser.CurrentToken.Position));
    }

    private void ValidateFunctionModifiers(SyntaxParser parser, List<AccessModifier> modifiers, SourceRange nameIdentifierRange)
    {
      if (modifiers.Count > 10)
      {
        parser.Error(Errors.TooManyIncompatibleModifiers, nameIdentifierRange);

        return;
      }

      modifiers.Reverse();

      List<AccessModifier> distinctModifiers = modifiers.DistinctBy((modifier) => modifier.Kind).ToList();

      if (distinctModifiers.Count != modifiers.Count)
      {
        foreach (AccessModifier modifier in modifiers.Except(distinctModifiers).Skip(1))
        {
          parser.ModifierError(Errors.ModifierAlreadySeen, modifier, modifier.ToString());
        }
      }

      foreach (AccessModifier modifier in modifiers)
      {
        // Static is an exception, it can be used alongside any other modifier
        if (modifier.Kind == AccessModifierKind.Static)
        {
          continue;
        }

        foreach (AccessModifierKind kind in new[] {AccessModifierKind.Public, AccessModifierKind.Internal, AccessModifierKind.Protected, AccessModifierKind.Private})
        {
          List<AccessModifier> incompatibleModifiers = modifiers.FindAll((m) => m.Kind == kind && m != modifier);

          foreach (AccessModifier incompatibleModifier in incompatibleModifiers)
          {
            if (modifier.Kind != incompatibleModifier.Kind)
            {
              parser.ModifierError(Errors.IncombinableModifier, modifier, modifier.ToString(), incompatibleModifier.ToString());

              break;
            }
          }
        }
      }
    }
  }
}