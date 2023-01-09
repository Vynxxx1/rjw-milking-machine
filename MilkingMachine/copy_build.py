#!/usr/bin/env python3

# not remotely idiomatic in python, all i can say is stay mad 🤷‍

from os.path import abspath, dirname
from pathlib import Path
from shutil import copy2 as cp
from glob import glob

def main():
    sln_dir = Path(dirname(abspath(__file__)))
    build_dir = sln_dir / "bin" / "Release"
    assemblies_dir = sln_dir / ".." / "1.4" / "Assemblies"

    for path in [Path(x) for x in glob(str(build_dir / "*.dll"))]:
        cp(path, assemblies_dir)

if __name__ == "__main__":
    main()
