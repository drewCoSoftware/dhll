lexer grammar t2Lexer;

// # TEMP:
WS: [ \r\n\t] -> skip;


EXP_OPEN: '{' ->pushMode(EXPRESSION);

// ==== EXPRESSION MODE ===========
mode EXPRESSION;

EXP_CLOSE: '}' -> popMode;

DOUBLE_QUOTED_STRING: DBL_QUOTE ~["]* DBL_QUOTE;
fragment DBL_QUOTE: '"';


OPEN_PAREN: '(';
CLOSE_PAREN: ')';

ASSIGN: EQUALS;
EQUALS: '=';


// OPERATOR: PLUS | MULT | DIVIDE | MINUS;
PLUS: '+';
MINUS: '-';
MULT: '*';
DIVIDE: '/';

// NOTE: don't try to use this kind of token, a lexer rule is better!
// NUMBER: INT | REAL;
// --> do this ... number : INT | REAL;
INT: DIGIT+;
REAL: DIGIT+? '.' DIGIT+;

fragment DIGIT: [0-9];

// Expressions don't care about whitespace.
EXP_WS: [ \r\n\t] -> skip;

// Catch leftover inputs that may not be processed otherwise....
ANY: .;