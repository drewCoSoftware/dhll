//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.2
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from ./v1/dhll.g4 by ANTLR 4.13.2

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace dhll.v1 {
using Antlr4.Runtime.Misc;
using IParseTreeListener = Antlr4.Runtime.Tree.IParseTreeListener;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete listener for a parse tree produced by
/// <see cref="dhllParser"/>.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.2")]
[System.CLSCompliant(false)]
public interface IdhllListener : IParseTreeListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="dhllParser.file"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFile([NotNull] dhllParser.FileContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="dhllParser.file"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFile([NotNull] dhllParser.FileContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="dhllParser.inlineComment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterInlineComment([NotNull] dhllParser.InlineCommentContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="dhllParser.inlineComment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitInlineComment([NotNull] dhllParser.InlineCommentContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="dhllParser.typedef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTypedef([NotNull] dhllParser.TypedefContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="dhllParser.typedef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTypedef([NotNull] dhllParser.TypedefContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="dhllParser.decl"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterDecl([NotNull] dhllParser.DeclContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="dhllParser.decl"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitDecl([NotNull] dhllParser.DeclContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="dhllParser.initializer"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterInitializer([NotNull] dhllParser.InitializerContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="dhllParser.initializer"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitInitializer([NotNull] dhllParser.InitializerContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="dhllParser.prop"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterProp([NotNull] dhllParser.PropContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="dhllParser.prop"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitProp([NotNull] dhllParser.PropContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="dhllParser.value"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterValue([NotNull] dhllParser.ValueContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="dhllParser.value"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitValue([NotNull] dhllParser.ValueContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="dhllParser.identifier"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterIdentifier([NotNull] dhllParser.IdentifierContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="dhllParser.identifier"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitIdentifier([NotNull] dhllParser.IdentifierContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="dhllParser.typename"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTypename([NotNull] dhllParser.TypenameContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="dhllParser.typename"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTypename([NotNull] dhllParser.TypenameContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="dhllParser.scope"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterScope([NotNull] dhllParser.ScopeContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="dhllParser.scope"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitScope([NotNull] dhllParser.ScopeContext context);
}
} // namespace dhll.v1
