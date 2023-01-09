#!/usr/bin/env python3

from datetime import datetime
from pathlib import Path
from os import mkdir, walk
from os.path import dirname, join
from glob import glob
from zipfile import ZipFile

MOD_NAME: str = "MilkingMachine"

def main():
    date = datetime.now().strftime("%Y.%m.%d")
    base_dir = (Path(dirname(__file__)) / "..").absolute()
    releases_dir = base_dir / "Releases"
    if not releases_dir.exists():
        mkdir(releases_dir)
    releases_today = len(glob(str(releases_dir / (date + "*.zip"))))
    zip_path = releases_dir / (date + chr(97 + releases_today) + ".zip")
    
    with ZipFile(zip_path, 'w') as zip_file:
        add_dir_to_zip(zip_file, base_dir, "About")
        add_dir_to_zip(zip_file, base_dir, "1.4")
        add_dir_to_zip(zip_file, base_dir, "Textures")

def add_dir_to_zip(zip_file: ZipFile, base_dir: Path, dir_name: str):
    for folder_name, _, file_names in walk(base_dir / dir_name):
        if len(file_names) == 0:
            continue

        arc_base = MOD_NAME + folder_name[len(str(base_dir)):]
        for file_name in file_names:
            zip_file.write(join(folder_name, file_name), join(arc_base, file_name))

if __name__ == '__main__':
    main()