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
using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.2")]
[System.CLSCompliant(false)]
public partial class dhllParser : Parser {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		T__0=1, ASSIGN=2, INT=3, FALSE=4, TRUE=5, PUBLIC=6, PRIVATE=7, PROP=8, 
		FIRSTCHAR=9, ID=10, WORD=11, SLASH=12, COMMENT=13, QUOTE=14, STRING=15, 
		OBRACE=16, CBRACE=17, EOS=18, LF=19, WS=20;
	public const int
		RULE_file = 0, RULE_inlineComment = 1, RULE_typedef = 2, RULE_decl = 3, 
		RULE_initializer = 4, RULE_prop = 5, RULE_value = 6, RULE_identifier = 7, 
		RULE_typename = 8, RULE_scope = 9;
	public static readonly string[] ruleNames = {
		"file", "inlineComment", "typedef", "decl", "initializer", "prop", "value", 
		"identifier", "typename", "scope"
	};

	private static readonly string[] _LiteralNames = {
		null, "'typedef'", "'='", null, "'false'", "'true'", "'public'", "'private'", 
		"'prop'", null, null, null, "'/'", null, "'\"'", null, "'{'", "'}'", "';'"
	};
	private static readonly string[] _SymbolicNames = {
		null, null, "ASSIGN", "INT", "FALSE", "TRUE", "PUBLIC", "PRIVATE", "PROP", 
		"FIRSTCHAR", "ID", "WORD", "SLASH", "COMMENT", "QUOTE", "STRING", "OBRACE", 
		"CBRACE", "EOS", "LF", "WS"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "dhll.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override int[] SerializedAtn { get { return _serializedATN; } }

	static dhllParser() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}

		public dhllParser(ITokenStream input) : this(input, Console.Out, Console.Error) { }

		public dhllParser(ITokenStream input, TextWriter output, TextWriter errorOutput)
		: base(input, output, errorOutput)
	{
		Interpreter = new ParserATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	public partial class FileContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public InlineCommentContext inlineComment() {
			return GetRuleContext<InlineCommentContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode Eof() { return GetToken(dhllParser.Eof, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public TypedefContext[] typedef() {
			return GetRuleContexts<TypedefContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public TypedefContext typedef(int i) {
			return GetRuleContext<TypedefContext>(i);
		}
		public FileContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_file; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IdhllListener typedListener = listener as IdhllListener;
			if (typedListener != null) typedListener.EnterFile(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IdhllListener typedListener = listener as IdhllListener;
			if (typedListener != null) typedListener.ExitFile(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IdhllVisitor<TResult> typedVisitor = visitor as IdhllVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitFile(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public FileContext file() {
		FileContext _localctx = new FileContext(Context, State);
		EnterRule(_localctx, 0, RULE_file);
		try {
			int _alt;
			State = 28;
			ErrorHandler.Sync(this);
			switch (TokenStream.LA(1)) {
			case COMMENT:
				EnterOuterAlt(_localctx, 1);
				{
				State = 20;
				inlineComment();
				}
				break;
			case T__0:
				EnterOuterAlt(_localctx, 2);
				{
				State = 22;
				ErrorHandler.Sync(this);
				_alt = 1+1;
				do {
					switch (_alt) {
					case 1+1:
						{
						{
						State = 21;
						typedef();
						}
						}
						break;
					default:
						throw new NoViableAltException(this);
					}
					State = 24;
					ErrorHandler.Sync(this);
					_alt = Interpreter.AdaptivePredict(TokenStream,0,Context);
				} while ( _alt!=1 && _alt!=global::Antlr4.Runtime.Atn.ATN.INVALID_ALT_NUMBER );
				State = 26;
				Match(Eof);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class InlineCommentContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode COMMENT() { return GetToken(dhllParser.COMMENT, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode LF() { return GetToken(dhllParser.LF, 0); }
		public InlineCommentContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_inlineComment; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IdhllListener typedListener = listener as IdhllListener;
			if (typedListener != null) typedListener.EnterInlineComment(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IdhllListener typedListener = listener as IdhllListener;
			if (typedListener != null) typedListener.ExitInlineComment(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IdhllVisitor<TResult> typedVisitor = visitor as IdhllVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitInlineComment(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public InlineCommentContext inlineComment() {
		InlineCommentContext _localctx = new InlineCommentContext(Context, State);
		EnterRule(_localctx, 2, RULE_inlineComment);
		try {
			int _alt;
			EnterOuterAlt(_localctx, 1);
			{
			State = 30;
			Match(COMMENT);
			{
			State = 34;
			ErrorHandler.Sync(this);
			_alt = Interpreter.AdaptivePredict(TokenStream,2,Context);
			while ( _alt!=1 && _alt!=global::Antlr4.Runtime.Atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1+1 ) {
					{
					{
					State = 31;
					MatchWildcard();
					}
					} 
				}
				State = 36;
				ErrorHandler.Sync(this);
				_alt = Interpreter.AdaptivePredict(TokenStream,2,Context);
			}
			}
			State = 37;
			Match(LF);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class TypedefContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public IdentifierContext identifier() {
			return GetRuleContext<IdentifierContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode OBRACE() { return GetToken(dhllParser.OBRACE, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode CBRACE() { return GetToken(dhllParser.CBRACE, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public DeclContext[] decl() {
			return GetRuleContexts<DeclContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public DeclContext decl(int i) {
			return GetRuleContext<DeclContext>(i);
		}
		public TypedefContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_typedef; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IdhllListener typedListener = listener as IdhllListener;
			if (typedListener != null) typedListener.EnterTypedef(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IdhllListener typedListener = listener as IdhllListener;
			if (typedListener != null) typedListener.ExitTypedef(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IdhllVisitor<TResult> typedVisitor = visitor as IdhllVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitTypedef(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public TypedefContext typedef() {
		TypedefContext _localctx = new TypedefContext(Context, State);
		EnterRule(_localctx, 4, RULE_typedef);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 39;
			Match(T__0);
			State = 40;
			identifier();
			State = 41;
			Match(OBRACE);
			State = 47;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			if (_la==PROP || _la==ID) {
				{
				State = 43;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
				do {
					{
					{
					State = 42;
					decl();
					}
					}
					State = 45;
					ErrorHandler.Sync(this);
					_la = TokenStream.LA(1);
				} while ( _la==PROP || _la==ID );
				}
			}

			State = 49;
			Match(CBRACE);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class DeclContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public TypenameContext typename() {
			return GetRuleContext<TypenameContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public IdentifierContext identifier() {
			return GetRuleContext<IdentifierContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode EOS() { return GetToken(dhllParser.EOS, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public PropContext prop() {
			return GetRuleContext<PropContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public InitializerContext initializer() {
			return GetRuleContext<InitializerContext>(0);
		}
		public DeclContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_decl; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IdhllListener typedListener = listener as IdhllListener;
			if (typedListener != null) typedListener.EnterDecl(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IdhllListener typedListener = listener as IdhllListener;
			if (typedListener != null) typedListener.ExitDecl(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IdhllVisitor<TResult> typedVisitor = visitor as IdhllVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitDecl(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public DeclContext decl() {
		DeclContext _localctx = new DeclContext(Context, State);
		EnterRule(_localctx, 6, RULE_decl);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 52;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			if (_la==PROP) {
				{
				State = 51;
				prop();
				}
			}

			State = 54;
			typename();
			State = 55;
			identifier();
			State = 57;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			if (_la==ASSIGN) {
				{
				State = 56;
				initializer();
				}
			}

			State = 59;
			Match(EOS);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class InitializerContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode ASSIGN() { return GetToken(dhllParser.ASSIGN, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ValueContext value() {
			return GetRuleContext<ValueContext>(0);
		}
		public InitializerContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_initializer; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IdhllListener typedListener = listener as IdhllListener;
			if (typedListener != null) typedListener.EnterInitializer(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IdhllListener typedListener = listener as IdhllListener;
			if (typedListener != null) typedListener.ExitInitializer(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IdhllVisitor<TResult> typedVisitor = visitor as IdhllVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitInitializer(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public InitializerContext initializer() {
		InitializerContext _localctx = new InitializerContext(Context, State);
		EnterRule(_localctx, 8, RULE_initializer);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 61;
			Match(ASSIGN);
			State = 62;
			value();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class PropContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode PROP() { return GetToken(dhllParser.PROP, 0); }
		public PropContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_prop; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IdhllListener typedListener = listener as IdhllListener;
			if (typedListener != null) typedListener.EnterProp(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IdhllListener typedListener = listener as IdhllListener;
			if (typedListener != null) typedListener.ExitProp(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IdhllVisitor<TResult> typedVisitor = visitor as IdhllVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitProp(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public PropContext prop() {
		PropContext _localctx = new PropContext(Context, State);
		EnterRule(_localctx, 10, RULE_prop);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 64;
			Match(PROP);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class ValueContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode INT() { return GetToken(dhllParser.INT, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode TRUE() { return GetToken(dhllParser.TRUE, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode FALSE() { return GetToken(dhllParser.FALSE, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode STRING() { return GetToken(dhllParser.STRING, 0); }
		public ValueContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_value; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IdhllListener typedListener = listener as IdhllListener;
			if (typedListener != null) typedListener.EnterValue(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IdhllListener typedListener = listener as IdhllListener;
			if (typedListener != null) typedListener.ExitValue(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IdhllVisitor<TResult> typedVisitor = visitor as IdhllVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitValue(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public ValueContext value() {
		ValueContext _localctx = new ValueContext(Context, State);
		EnterRule(_localctx, 12, RULE_value);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 66;
			_la = TokenStream.LA(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & 32824L) != 0)) ) {
			ErrorHandler.RecoverInline(this);
			}
			else {
				ErrorHandler.ReportMatch(this);
			    Consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class IdentifierContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode ID() { return GetToken(dhllParser.ID, 0); }
		public IdentifierContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_identifier; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IdhllListener typedListener = listener as IdhllListener;
			if (typedListener != null) typedListener.EnterIdentifier(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IdhllListener typedListener = listener as IdhllListener;
			if (typedListener != null) typedListener.ExitIdentifier(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IdhllVisitor<TResult> typedVisitor = visitor as IdhllVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitIdentifier(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public IdentifierContext identifier() {
		IdentifierContext _localctx = new IdentifierContext(Context, State);
		EnterRule(_localctx, 14, RULE_identifier);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 68;
			Match(ID);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class TypenameContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public IdentifierContext identifier() {
			return GetRuleContext<IdentifierContext>(0);
		}
		public TypenameContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_typename; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IdhllListener typedListener = listener as IdhllListener;
			if (typedListener != null) typedListener.EnterTypename(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IdhllListener typedListener = listener as IdhllListener;
			if (typedListener != null) typedListener.ExitTypename(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IdhllVisitor<TResult> typedVisitor = visitor as IdhllVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitTypename(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public TypenameContext typename() {
		TypenameContext _localctx = new TypenameContext(Context, State);
		EnterRule(_localctx, 16, RULE_typename);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 70;
			identifier();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class ScopeContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode PUBLIC() { return GetToken(dhllParser.PUBLIC, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode PRIVATE() { return GetToken(dhllParser.PRIVATE, 0); }
		public ScopeContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_scope; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IdhllListener typedListener = listener as IdhllListener;
			if (typedListener != null) typedListener.EnterScope(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IdhllListener typedListener = listener as IdhllListener;
			if (typedListener != null) typedListener.ExitScope(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IdhllVisitor<TResult> typedVisitor = visitor as IdhllVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitScope(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public ScopeContext scope() {
		ScopeContext _localctx = new ScopeContext(Context, State);
		EnterRule(_localctx, 18, RULE_scope);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 72;
			_la = TokenStream.LA(1);
			if ( !(_la==PUBLIC || _la==PRIVATE) ) {
			ErrorHandler.RecoverInline(this);
			}
			else {
				ErrorHandler.ReportMatch(this);
			    Consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	private static int[] _serializedATN = {
		4,1,20,75,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,2,5,7,5,2,6,7,6,2,7,
		7,7,2,8,7,8,2,9,7,9,1,0,1,0,4,0,23,8,0,11,0,12,0,24,1,0,1,0,3,0,29,8,0,
		1,1,1,1,5,1,33,8,1,10,1,12,1,36,9,1,1,1,1,1,1,2,1,2,1,2,1,2,4,2,44,8,2,
		11,2,12,2,45,3,2,48,8,2,1,2,1,2,1,3,3,3,53,8,3,1,3,1,3,1,3,3,3,58,8,3,
		1,3,1,3,1,4,1,4,1,4,1,5,1,5,1,6,1,6,1,7,1,7,1,8,1,8,1,9,1,9,1,9,2,24,34,
		0,10,0,2,4,6,8,10,12,14,16,18,0,2,2,0,3,5,15,15,1,0,6,7,71,0,28,1,0,0,
		0,2,30,1,0,0,0,4,39,1,0,0,0,6,52,1,0,0,0,8,61,1,0,0,0,10,64,1,0,0,0,12,
		66,1,0,0,0,14,68,1,0,0,0,16,70,1,0,0,0,18,72,1,0,0,0,20,29,3,2,1,0,21,
		23,3,4,2,0,22,21,1,0,0,0,23,24,1,0,0,0,24,25,1,0,0,0,24,22,1,0,0,0,25,
		26,1,0,0,0,26,27,5,0,0,1,27,29,1,0,0,0,28,20,1,0,0,0,28,22,1,0,0,0,29,
		1,1,0,0,0,30,34,5,13,0,0,31,33,9,0,0,0,32,31,1,0,0,0,33,36,1,0,0,0,34,
		35,1,0,0,0,34,32,1,0,0,0,35,37,1,0,0,0,36,34,1,0,0,0,37,38,5,19,0,0,38,
		3,1,0,0,0,39,40,5,1,0,0,40,41,3,14,7,0,41,47,5,16,0,0,42,44,3,6,3,0,43,
		42,1,0,0,0,44,45,1,0,0,0,45,43,1,0,0,0,45,46,1,0,0,0,46,48,1,0,0,0,47,
		43,1,0,0,0,47,48,1,0,0,0,48,49,1,0,0,0,49,50,5,17,0,0,50,5,1,0,0,0,51,
		53,3,10,5,0,52,51,1,0,0,0,52,53,1,0,0,0,53,54,1,0,0,0,54,55,3,16,8,0,55,
		57,3,14,7,0,56,58,3,8,4,0,57,56,1,0,0,0,57,58,1,0,0,0,58,59,1,0,0,0,59,
		60,5,18,0,0,60,7,1,0,0,0,61,62,5,2,0,0,62,63,3,12,6,0,63,9,1,0,0,0,64,
		65,5,8,0,0,65,11,1,0,0,0,66,67,7,0,0,0,67,13,1,0,0,0,68,69,5,10,0,0,69,
		15,1,0,0,0,70,71,3,14,7,0,71,17,1,0,0,0,72,73,7,1,0,0,73,19,1,0,0,0,7,
		24,28,34,45,47,52,57
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
} // namespace dhll.v1
