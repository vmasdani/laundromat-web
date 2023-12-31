#!/usr/bin/python3

import subprocess
import argparse

parser = argparse.ArgumentParser()
parser.add_argument("action")

args = parser.parse_args()

steps = []

if args.action == "run":
    steps = [
        ("yarn build --mode development", "./frontend"),
        ("rm -rf wwwroot/* && cp -r frontend/dist/* wwwroot", "."),
        ("dotnet run", ".")
    ]


elif args.action == "build":
    steps = [
        ("yarn build", "./frontend"),
        ("dotnet build -r linux-x64 && cp -r wwwroot bin/Debug/net6.0/linux-x64/", "."),
        ("dotnet build -r win-x64 && cp -r wwwroot bin/Debug/net6.0/win-x64/", ".")

    ]


for s, cwd in steps:
    subprocess.run(s, cwd=cwd, shell=True)
