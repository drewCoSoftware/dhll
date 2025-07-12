# A way to easily test our grammars as we develop them.  Basically we expect ANTLR to return a 0 when it can do its thing correctly.
import subprocess

# import os
# print(os.getcwd())

TEST_INPUT = "test-file.txt"
P_GRAMMAR = "./Grammars/v1/t2Parser.g4"
L_GRAMMAR = "./Grammars/v1/t2Lexer.g4"
CMD = "antlr4-parse"

# UMMM:: THAT needs to be fixed, lol.....
# CMD = r"C:\%HOMEPATH%\.pyenv\pyenv-win\shims\antlr4-parse.bat"
CMD = r"C:\users\drew\.pyenv\pyenv-win\shims\antlr4-parse.bat"


TESTS = [
    ("expression", "{123}", "Expression: Literal Number"),
    ("expression", "{123+456}", "Expression: Add Numbers"),
    ("expression", "{123 + 456}", "Expression: Add Numbers w/ space"),
]

# OPTIONS:
# Stop as soon as a test case fails!
QUIT_EARLY = True


# -----------------------------------------------------------------------------------------
# Analyze the parser output for errors.
def HasErrors(testoutput: str) -> str:
    # EZ MODE:  Assume that any error output is a failure!
    return len(testoutput) > 1


index = 0


# OPTIONS: - Test Filter
useTests = TESTS
max = len(useTests)

print(f"Detected: {max} tests")

for t in useTests:
    # print(t)
    # print(t[0])
    # print(t[1])

    index += 1

    name = f"#{index}"
    if len(t) > 2:
        name = t[2]

    # VEBOSE?
    print(f"TEST: {name}")

    with open(TEST_INPUT, "w") as inf:
        inf.write(t[1])

    TEST_OUTPUT = "output.txt"
    exe = f"{CMD} {P_GRAMMAR} {L_GRAMMAR} {t[0]} {TEST_INPUT} 2> {TEST_OUTPUT}"

    callRes = subprocess.call(exe)

    # Read the data back in and check for errors.
    with open(TEST_OUTPUT, "r") as outf:
        outdata = outf.read()
        if HasErrors(outdata):
            print(f"TEST: {name} failed!")

            # VERBOSE / RUNLOG?
            print(outdata)

            if QUIT_EARLY:
                print("Test execution will stop now!")
                break

    # TEMP
    # break
