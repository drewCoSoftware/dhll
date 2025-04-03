# Simple script to publish dhll to some local directory.
import subprocess


exe = "dotnet publish -c Release -o ./dist dhll/dhll.csproj"
callRes = subprocess.call(exe)
if callRes != 0:
    raise Exception("could not build dhll!")

