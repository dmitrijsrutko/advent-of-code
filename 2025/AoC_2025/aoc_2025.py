from Day05.Day05 import Day05
import time

if __name__ == "__main__":

    start = time.time()
    day = Day05("05")
    day.run()
    end = time.time()

    print(f"Execution time: {(end - start) * 1000:.1f} ms")
