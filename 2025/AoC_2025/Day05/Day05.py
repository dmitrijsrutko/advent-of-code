class Day05:
    def __init__(self, day: str):
        self.day = day

    def isValid(self, ranges: list((int, int)), id: int) -> bool:
        for (start, end) in ranges:
            if start <= id <= end:
                return True
            
        return False

    def solve_01(self, ranges: list((int, int)), ids: list(int)) -> int:
        count = 0
        for id in ids:
            if self.isValid(ranges, id):
                count += 1

        return count
    
    def find_left_most(self, ranges: list((int, int)), x: int) -> (int, int):
        min_start = -1
        min_end = -1

        for (start, end) in ranges:
            if (start >= x):
                if (min_start == -1) or (start < min_start):
                    min_start = start
                    min_end = end

        return (min_start, min_end)
    
    def find_right_most(self, ranges: list((int, int)), start: int, end: int) -> (int, int):
        for (r_start, r_end) in ranges:
            if (start <= r_start <= end):
                end = max(end, r_end)

        return (start, end)
    
    def solve_02(self, ranges: list((int, int))) -> int:
        count = 0
        x = 0

        while True:
            (start, end) = self.find_left_most(ranges, x)
            if (start, end) == (-1, -1):
                # no more ranges
                break

            while True:
                (n_start, n_end) = self.find_right_most(ranges, start, end)
                if (n_start, n_end) == (start, end):
                    # no extension
                    break
                else:
                    (start, end) = (n_start, n_end)

            count += end - start + 1
            x = end + 1

        return count

    def run(self):
        # filename = f"./Day{self.day}/test{self.day}.txt"
        filename = f"./Day{self.day}/data{self.day}.txt"

        with open(filename, 'r') as file:
            lines = file.readlines()

        for i in range(len(lines)):
            lines[i] = lines[i].strip()

        ranges = []
        ids = []

        was_break = False
        for line in lines:
            if not line:
                was_break = True
                continue

            if not was_break:
                range_parts = line.split('-')
                # convert to integers
                start = int(range_parts[0])
                end = int(range_parts[1])
                pair = (start, end)
                ranges.append(pair)
            else:
                id = int(line)
                ids.append(id)

        # result_01 = self.solve_01(ranges, ids)
        # print(result_01)

        result_02 = self.solve_02(ranges)
        print(result_02)
        pass

'''
756
Execution time: 4.3 ms

355555479253787
Execution time: 1.8 ms
'''