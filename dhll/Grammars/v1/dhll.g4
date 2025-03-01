grammar dhll;

file: (COMMENT | typedef)+? EOF;



// NOTE: Inline comments can't be used in typedefs... I think we probably need to fix that somehow so that they can go
// anywhere.....
typedef: 'typedef' identifier OBRACE (decl+)? CBRACE;

decl: prop? typename identifier initializer? EOS;

initializer: ASSIGN value;
prop:PROP;

value: INT | TRUE | FALSE | STRING;

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
ID: [a-zA-Z_]([a-zA-Z0-9_]*);     // FIX: This is making it so that we need at least two chars.  making the second part optional (?) makes it so no ids are detected....


WORD: [a-zA-Z]+;

SLASH: '/';
COMMENT: SLASH SLASH (.)*? '\n' ->channel(HIDDEN);

QUOTE: '"';
STRING: (QUOTE)(.*?)(QUOTE);


OBRACE: '{';
CBRACE: '}';


EOS: ';';       // end of statement (line)

LF: '\n' -> skip;
WS: [ \r\n\t] -> skip;