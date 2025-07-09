# Simple script to publish dhll to some local directory.
# With no options, this will publist to './dist'
# example:
# python publish.py  // default directory is ""./dist"
# python publish.py "c:\apps\dhll"

import subprocess
import sys

OUTPUT_DIR = './dist'
if len(sys.argv) > 1:
    OUTPUT_DIR = sys.argv[1]

exe = f"dotnet publish -c Release -o {OUTPUT_DIR} dhll/dhll.csproj"
callRes = subprocess.call(exe)
if callRes != 0:
    raise Exception("could not build dhll!")

