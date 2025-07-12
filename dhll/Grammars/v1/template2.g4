grammar template2;






expression
    : value 
    | value OPERATOR value
    ;

value: number;
number: INT | REAL;







// TODO: Make a lexer file?

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