//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.2
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from ./v1/TypeDef.g4 by ANTLR 4.13.2

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
/// <see cref="TypeDefParser"/>.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.2")]
[System.CLSCompliant(false)]
public interface ITypeDefListener : IParseTreeListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="TypeDefParser.file"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFile([NotNull] TypeDefParser.FileContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TypeDefParser.file"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFile([NotNull] TypeDefParser.FileContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TypeDefParser.typedef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTypedef([NotNull] TypeDefParser.TypedefContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TypeDefParser.typedef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTypedef([NotNull] TypeDefParser.TypedefContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TypeDefParser.decl"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterDecl([NotNull] TypeDefParser.DeclContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TypeDefParser.decl"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitDecl([NotNull] TypeDefParser.DeclContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TypeDefParser.initializer"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterInitializer([NotNull] TypeDefParser.InitializerContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TypeDefParser.initializer"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitInitializer([NotNull] TypeDefParser.InitializerContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TypeDefParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterExpr([NotNull] TypeDefParser.ExprContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TypeDefParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitExpr([NotNull] TypeDefParser.ExprContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TypeDefParser.identifier"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterIdentifier([NotNull] TypeDefParser.IdentifierContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TypeDefParser.identifier"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitIdentifier([NotNull] TypeDefParser.IdentifierContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TypeDefParser.typename"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTypename([NotNull] TypeDefParser.TypenameContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TypeDefParser.typename"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTypename([NotNull] TypeDefParser.TypenameContext context);
}
} // namespace dhll.v1
