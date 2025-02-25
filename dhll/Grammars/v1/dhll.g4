grammar dhll;

file: typedef+? EOF;

typedef: 'typedef' identifier OBRACE (decl+)? CBRACE;

decl: prop? typename identifier initializer? EOS;

initializer: ASSIGN expr;
prop:PROP;

expr: INT | TRUE | FALSE | STRING;

identifier: ID;
typename: identifier;

scope: PUBLIC | PRIVATE;

ASSIGN: '=';
INT: [0-9]+;

FALSE: 'false';
TRUE: 'true';


PUBLIC: 'public';
PRIVATE: 'private';

PROP: 'prop';           // Indicates a declaration should be implemented as a property.

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