# DHLL : Drew's High Level Language
I am under the impression that I have programmed for long enough that I can create my own silly little high-level language, and use that to squirt
out code to a variety of platforms.  Let's see if I am actually good at computers, or completely deluded by my own hubris!


## Goals
- Simple syntax that makes programming the constructs that I care about the most easy to do.  
- Emit / codegent to other languages, like C#, C/C++, typescript, etc.
- Our own native compiler would be rad, but I am only a little bit crazy at this point.









### NOTES:
Use this CLI syntax for parser output:
```
// NOTE: This somehow outputs the files to ParserLexer/v1, which totally works for me.....
// NOTE: Don't forget the 'package' argument so it all goes in the right namespace.
cd Grammars
antlr4 -package dhll.v1 -visitor -Dlanguage=CSharp -o ./v1/ParserLexer ./v1/TypeDef.g4 
```

To test some rules:
```
// Make sure to point to the correct file version.
antlr4-parse ./v1/TypeDef.g4 file -gui test-input.txt
```