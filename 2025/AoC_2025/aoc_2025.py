from Day12.Day12 import Day12

import time

if __name__ == "__main__":

    start = time.time()
    day = Day12("12")
    day.run()
    end = time.time()

    print(f"Execution time: {(end - start) * 1000:.1f} ms")
