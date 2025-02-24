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
using IErrorNode = Antlr4.Runtime.Tree.IErrorNode;
using ITerminalNode = Antlr4.Runtime.Tree.ITerminalNode;
using IToken = Antlr4.Runtime.IToken;
using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;

/// <summary>
/// This class provides an empty implementation of <see cref="ITypeDefListener"/>,
/// which can be extended to create a listener which only needs to handle a subset
/// of the available methods.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.2")]
[System.Diagnostics.DebuggerNonUserCode]
[System.CLSCompliant(false)]
public partial class TypeDefBaseListener : ITypeDefListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="TypeDefParser.file"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterFile([NotNull] TypeDefParser.FileContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TypeDefParser.file"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitFile([NotNull] TypeDefParser.FileContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TypeDefParser.typedef"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterTypedef([NotNull] TypeDefParser.TypedefContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TypeDefParser.typedef"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitTypedef([NotNull] TypeDefParser.TypedefContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TypeDefParser.decl"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterDecl([NotNull] TypeDefParser.DeclContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TypeDefParser.decl"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitDecl([NotNull] TypeDefParser.DeclContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TypeDefParser.initializer"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterInitializer([NotNull] TypeDefParser.InitializerContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TypeDefParser.initializer"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitInitializer([NotNull] TypeDefParser.InitializerContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TypeDefParser.expr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterExpr([NotNull] TypeDefParser.ExprContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TypeDefParser.expr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitExpr([NotNull] TypeDefParser.ExprContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TypeDefParser.identifier"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterIdentifier([NotNull] TypeDefParser.IdentifierContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TypeDefParser.identifier"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitIdentifier([NotNull] TypeDefParser.IdentifierContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TypeDefParser.typename"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterTypename([NotNull] TypeDefParser.TypenameContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TypeDefParser.typename"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitTypename([NotNull] TypeDefParser.TypenameContext context) { }

	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void EnterEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void ExitEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitTerminal([NotNull] ITerminalNode node) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitErrorNode([NotNull] IErrorNode node) { }
}
} // namespace dhll.v1
