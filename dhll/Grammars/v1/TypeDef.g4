grammar TypeDef;

file: typedef+? EOF;

typedef: 'typedef' identifier OBRACE (decl+)? CBRACE;

decl: typename identifier initializer? EOS;

initializer: ASSIGN expr;

expr: WORD | INT | TRUE | FALSE;

identifier: WORD;
typename: identifier;

ASSIGN: '=';
INT: [0-9]+;


WORD: [a-zA-Z]+;

FALSE: 'false';
TRUE: 'true';

OBRACE: '{';
CBRACE: '}';

// end of statement (line)
EOS: ';';

WS: [ \r\n\t] -> skip;