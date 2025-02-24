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
using System;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.2")]
[System.CLSCompliant(false)]
public partial class TypeDefLexer : Lexer {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		T__0=1, ASSIGN=2, INT=3, WORD=4, FALSE=5, TRUE=6, OBRACE=7, CBRACE=8, 
		EOS=9, WS=10;
	public static string[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"T__0", "ASSIGN", "INT", "WORD", "FALSE", "TRUE", "OBRACE", "CBRACE", 
		"EOS", "WS"
	};


	public TypeDefLexer(ICharStream input)
	: this(input, Console.Out, Console.Error) { }

	public TypeDefLexer(ICharStream input, TextWriter output, TextWriter errorOutput)
	: base(input, output, errorOutput)
	{
		Interpreter = new LexerATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	private static readonly string[] _LiteralNames = {
		null, "'typedef'", "'='", null, null, "'false'", "'true'", "'{'", "'}'", 
		"';'"
	};
	private static readonly string[] _SymbolicNames = {
		null, null, "ASSIGN", "INT", "WORD", "FALSE", "TRUE", "OBRACE", "CBRACE", 
		"EOS", "WS"
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

	public override string GrammarFileName { get { return "TypeDef.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string[] ChannelNames { get { return channelNames; } }

	public override string[] ModeNames { get { return modeNames; } }

	public override int[] SerializedAtn { get { return _serializedATN; } }

	static TypeDefLexer() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}
	private static int[] _serializedATN = {
		4,0,10,62,6,-1,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,2,5,7,5,2,6,7,6,
		2,7,7,7,2,8,7,8,2,9,7,9,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,1,1,1,1,2,4,
		2,33,8,2,11,2,12,2,34,1,3,4,3,38,8,3,11,3,12,3,39,1,4,1,4,1,4,1,4,1,4,
		1,4,1,5,1,5,1,5,1,5,1,5,1,6,1,6,1,7,1,7,1,8,1,8,1,9,1,9,1,9,1,9,0,0,10,
		1,1,3,2,5,3,7,4,9,5,11,6,13,7,15,8,17,9,19,10,1,0,3,1,0,48,57,2,0,65,90,
		97,122,3,0,9,10,13,13,32,32,63,0,1,1,0,0,0,0,3,1,0,0,0,0,5,1,0,0,0,0,7,
		1,0,0,0,0,9,1,0,0,0,0,11,1,0,0,0,0,13,1,0,0,0,0,15,1,0,0,0,0,17,1,0,0,
		0,0,19,1,0,0,0,1,21,1,0,0,0,3,29,1,0,0,0,5,32,1,0,0,0,7,37,1,0,0,0,9,41,
		1,0,0,0,11,47,1,0,0,0,13,52,1,0,0,0,15,54,1,0,0,0,17,56,1,0,0,0,19,58,
		1,0,0,0,21,22,5,116,0,0,22,23,5,121,0,0,23,24,5,112,0,0,24,25,5,101,0,
		0,25,26,5,100,0,0,26,27,5,101,0,0,27,28,5,102,0,0,28,2,1,0,0,0,29,30,5,
		61,0,0,30,4,1,0,0,0,31,33,7,0,0,0,32,31,1,0,0,0,33,34,1,0,0,0,34,32,1,
		0,0,0,34,35,1,0,0,0,35,6,1,0,0,0,36,38,7,1,0,0,37,36,1,0,0,0,38,39,1,0,
		0,0,39,37,1,0,0,0,39,40,1,0,0,0,40,8,1,0,0,0,41,42,5,102,0,0,42,43,5,97,
		0,0,43,44,5,108,0,0,44,45,5,115,0,0,45,46,5,101,0,0,46,10,1,0,0,0,47,48,
		5,116,0,0,48,49,5,114,0,0,49,50,5,117,0,0,50,51,5,101,0,0,51,12,1,0,0,
		0,52,53,5,123,0,0,53,14,1,0,0,0,54,55,5,125,0,0,55,16,1,0,0,0,56,57,5,
		59,0,0,57,18,1,0,0,0,58,59,7,2,0,0,59,60,1,0,0,0,60,61,6,9,0,0,61,20,1,
		0,0,0,3,0,34,39,1,6,0,0
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
} // namespace dhll.v1
