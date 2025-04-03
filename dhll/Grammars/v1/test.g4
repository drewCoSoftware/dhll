// This file  / gammar is here so that we might test some other features....
grammar test;


statement: expression ASSIGN expression;

expression: DIGIT | 
            value OPERATOR value;


value: NUMBER;


OPERATOR: PLUS | ASSIGN | MULT | DIVIDE;

PLUS: '+';
ASSIGN: '=';
MULT: '*';
DIVIDE: '/';

NUMBER: DIGIT | INT;

DIGIT: [0-9];
INT: DIGIT+;




LF: '\n' -> skip;
WS: [ \r\n\t] -> skip;


