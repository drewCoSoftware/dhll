lexer grammar t2Lexer;

// ==== EXPRESSION RULES ===========

 EXP_OPEN: '{' ->pushMode(EXPRESSION);

mode EXPRESSION;

EXP_CLOSE: '}' -> popMode;

ASSIGN: EQUALS;
EQUALS: '=';


OPERATOR: PLUS | MULT | DIVIDE;
PLUS: '+';
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
