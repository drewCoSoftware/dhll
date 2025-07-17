// NOTE: A good bit of this was cribbed from https://github.com/antlr/grammars-v4/blob/master/html
// I didn't just copy it all verbatim as this is also a learning opportunity for me.
lexer grammar templateLexer;

// DOUBLE_QUOTED_STRING: DBL_QUOTE ~["]* DBL_QUOTE;
//fragment DBL_QUOTE: '"';

SEA_WS: (' ' | '\t' | '\r'? '\n')+;

TAG_OPEN: '<' ->pushMode(TAG);

HTML_TEXT: ~'<'+;

// TAG MODE ------------------------------------------------------------------------------------------
mode TAG;

TAG_CLOSE: '>' -> popMode;
TAG_SLASH_CLOSE: '/>' -> popMode;
TAG_SLASH: '/';

TAG_EQUALS: '='; // -> pushMode(ATTVAL);

TAG_NAME: TAG_NameStartChar TAG_NameChar*;
TAG_WHITESPACE: [ \t\r\n] -> channel(HIDDEN);

fragment HEXDIGIT: [a-fA-F0-9];

fragment DIGIT: [0-9];

fragment TAG_NameChar:
    TAG_NameStartChar
    | '-'
    | '_'
    | '.'
    | DIGIT
    | '\u00B7'
    | '\u0300' ..'\u036F'
    | '\u203F' ..'\u2040'
;

fragment TAG_NameStartChar:
    [:a-zA-Z]
    | '\u2070' ..'\u218F'
    | '\u2C00' ..'\u2FEF'
    | '\u3001' ..'\uD7FF'
    | '\uF900' ..'\uFDCF'
    | '\uFDF0' ..'\uFFFD'
;

TAG_DBL_QUOTE: '"';
TAG_DQ_STR: TAG_DBL_QUOTE ~["]* TAG_DBL_QUOTE;


EXP_OPEN: '{' ->pushMode(EXPRESSION);

// ==== EXPRESSION MODE ===========
mode EXPRESSION;

EXP_CLOSE: '}' -> popMode;

// Operators
PLUS    : '+' ;
MINUS   : '-' ;
MULT    : '*' ;
DIVIDE  : '/' ;

// IDENTIFIER
ID      : [a-zA-Z_] [a-zA-Z_0-9]* ;

// Delimiters
OPEN_PAREN: '(';
CLOSE_PAREN: ')';
COMMA   : ',' ;

// NOTE: don't try to use this kind of token, a lexer rule is better!
// NUMBER: INT | REAL;
// --> do this ... number : INT | REAL;
INT: DIGIT+;
REAL: DIGIT+? '.' DIGIT+;
// fragment DIGIT: [0-9];

EXP_DBL_QUOTE: '"';
EXP_DQ_STR: TAG_DBL_QUOTE ~["]* TAG_DBL_QUOTE;

// Expressions don't care about whitespace.
EXP_WS: [ \r\n\t] -> skip;

// Catch leftover inputs that may not be processed otherwise....
ANY: .;


// // ATTRIBUTE VALUES MODE ------------------------------------------------------------------------------------------
// mode ATTVALUE;

// // an attribute value may have spaces b/t the '=' and the value
// ATTVALUE_VALUE: ' '* ATTRIBUTE -> popMode;

// ATTRIBUTE: DOUBLE_QUOTE_STRING | SINGLE_QUOTE_STRING | ATTCHARS | HEXCHARS | DECCHARS | PROP_STRING;

// fragment ATTCHARS: ATTCHAR+ ' '?;

// fragment ATTCHAR: '-' | '_' | '.' | '/' | '+' | ',' | '?' | '=' | ':' | ';' | '#' | [0-9a-zA-Z];

// fragment HEXCHARS: '#' [0-9a-fA-F]+;

// fragment DECCHARS: [0-9]+ '%'?;

// fragment DOUBLE_QUOTE_STRING: '"' ~[<"]* '"';

// fragment SINGLE_QUOTE_STRING: '\'' ~[<']* '\'';

// fragment PROP_STRING: '{' ~[<']* '}';


// // XML: '<?xml' .*? '>';

// // SCRIPT_OPEN: '<script' .*? '>' -> pushMode(SCRIPT);

// // TAG MODE ------------------------------------------------------------------------------------------
// mode TAG;

// TAG_CLOSE: '>' -> popMode;

// TAG_SLASH_CLOSE: '/>' -> popMode;

// TAG_SLASH: '/';

// // lexing mode for attribute values

// TAG_EQUALS: '=' -> pushMode(ATTVALUE);

// TAG_NAME: TAG_NameStartChar TAG_NameChar*;

// TAG_WHITESPACE: [ \t\r\n] -> channel(HIDDEN);

// fragment HEXDIGIT: [a-fA-F0-9];

// fragment DIGIT: [0-9];

// fragment TAG_NameChar:
//     TAG_NameStartChar
//     | '-'
//     | '_'
//     | '.'
//     | DIGIT
//     | '\u00B7'
//     | '\u0300' ..'\u036F'
//     | '\u203F' ..'\u2040'
// ;

// fragment TAG_NameStartChar:
//     [:a-zA-Z]
//     | '\u2070' ..'\u218F'
//     | '\u2C00' ..'\u2FEF'
//     | '\u3001' ..'\uD7FF'
//     | '\uF900' ..'\uFDCF'
//     | '\uFDF0' ..'\uFFFD'
// ;

// // SCRIPT MODE ------------------------------------------------------------------------------------------
// mode SCRIPT;

// SCRIPT_BODY: .*? '</script>' -> popMode;

// SCRIPT_SHORT_BODY: .*? '</>' -> popMode;



// // attribute values


