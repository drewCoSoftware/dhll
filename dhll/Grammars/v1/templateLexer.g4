// NOTE: A good bit of this was cribbed from https://github.com/antlr/grammars-v4/blob/master/html
// I didn't just copy it all verbatim as this is also a learning opportunity for me.
lexer grammar templateLexer;


// template: tag* EOF;
HTML_COMMENT: '<!--' .*? '-->';


// tag: TAG_OPEN TAG_NAME TAG_CLOSE;

// htmlElement
//     : TAG_OPEN TAG_NAME htmlAttribute* (
//         TAG_CLOSE (htmlContent TAG_OPEN TAG_SLASH TAG_NAME TAG_CLOSE)?
//         | TAG_SLASH_CLOSE
//     )
//     ;

// htmlAttribute
//     : TAG_NAME (TAG_EQUALS ATTVALUE_VALUE)?
//     ;


// htmlContent
//     : htmlChardata? ((htmlElement  htmlComment) htmlChardata?)*
//     ;

// htmlComment
//     : HTML_COMMENT
//     | HTML_CONDITIONAL_COMMENT
//     ;

// htmlAttribute
//     : TAG_NAME (TAG_EQUALS ATTVALUE_VALUE)?
//     ;
SEA_WS: (' ' | '\t' | '\r'? '\n')+;


TAG_OPEN: '<' ->pushMode(TAG);

HTML_TEXT: ~'<'+;

// TAG MODE ------------------------------------------------------------------------------------------
mode TAG;

TAG_CLOSE: '>' -> popMode;
TAG_SLASH_CLOSE: '/>' -> popMode;
TAG_SLASH: '/';

TAG_EQUALS: '=' -> pushMode(ATTVALUE);
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

// ATTRIBUTE VALUES MODE ------------------------------------------------------------------------------------------
mode ATTVALUE;

// an attribute value may have spaces b/t the '=' and the value
ATTVALUE_VALUE: ' '* ATTRIBUTE -> popMode;

ATTRIBUTE: DOUBLE_QUOTE_STRING | SINGLE_QUOTE_STRING | ATTCHARS | HEXCHARS | DECCHARS | PROP_STRING;

fragment ATTCHARS: ATTCHAR+ ' '?;

fragment ATTCHAR: '-' | '_' | '.' | '/' | '+' | ',' | '?' | '=' | ':' | ';' | '#' | [0-9a-zA-Z];

fragment HEXCHARS: '#' [0-9a-fA-F]+;

fragment DECCHARS: [0-9]+ '%'?;

fragment DOUBLE_QUOTE_STRING: '"' ~[<"]* '"';

fragment SINGLE_QUOTE_STRING: '\'' ~[<']* '\'';

fragment PROP_STRING: '{' ~[<']* '}';

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


