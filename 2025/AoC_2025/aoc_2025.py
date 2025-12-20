from Day06.Day06 import Day06

import time

if __name__ == "__main__":

    start = time.time()
    day = Day06("06")
    day.run()
    end = time.time()

    print(f"Execution time: {(end - start) * 1000:.1f} ms")
