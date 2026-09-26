import os
import sys

import cast

directory = sys.argv[1]
paths = [os.path.join(directory, name) for name in sorted(os.listdir(directory)) if name.endswith(".cast")]

if not paths:
    sys.exit(f"No cast files found in {directory}")

failures = 0

for path in paths:
    resaved = os.path.join(directory, os.path.basename(path) + ".python")
    cast.Cast.load(path).save(resaved)

    with open(path, "rb") as original, open(resaved, "rb") as python:
        if original.read() == python.read():
            print(f"OK: {os.path.basename(path)}")
        else:
            failures += 1
            print(f"MISMATCH: {os.path.basename(path)} differs after a load and save with the reference library")

sys.exit(1 if failures else 0)
