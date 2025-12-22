from Day11.Day11 import Day11

import time

if __name__ == "__main__":

    start = time.time()
    day = Day11("11")
    day.run()
    end = time.time()

    print(f"Execution time: {(end - start) * 1000:.1f} ms")
