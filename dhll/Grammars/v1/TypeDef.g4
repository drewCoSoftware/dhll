grammar TypeDef;

file: typedef+? EOF;

typedef: 'typedef' identifier OBRACE (decl+)? CBRACE;

decl: typename identifier initializer? EOS;

initializer: ASSIGN expr;

expr: INT | TRUE | FALSE | STRING;

identifier: ID;
typename: identifier;

ASSIGN: '=';
INT: [0-9]+;

FALSE: 'false';
TRUE: 'true';

FIRSTCHAR: [a-zA-Z_];
ID: (FIRSTCHAR)[a-zA-Z0-9_]+;


WORD: [a-zA-Z]+;


QUOTE: '"';
STRING: (QUOTE)(.*?)(QUOTE);


OBRACE: '{';
CBRACE: '}';

// end of statement (line)
EOS: ';';

WS: [ \r\n\t] -> skip;