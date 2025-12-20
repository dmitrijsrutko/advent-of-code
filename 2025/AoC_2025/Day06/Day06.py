class Day06:
    def __init__(self, day: str):
        self.day = day

    def solve_01(self, numbers: list[list[int]], ops: list[str]) -> int:

        total = 0
        for c in range(len(ops)):
            if (ops[c] == '+'):
                # addition
                sum = 0
                for r in range(len(numbers)):
                    sum += numbers[r][c]

                total += sum
                pass
            elif (ops[c] == '*'):
                # multiplication
                mult = 1
                for r in range(len(numbers)):
                    mult *= numbers[r][c]
                
                total += mult
                pass
            else:
                raise ValueError(f"Unknown operation: {ops[c]}")

        return total
    
    def solve_02(self, data: list[str]) -> int:
        lines = []
        for line in data:
            line = line.strip('\n')
            lines.append(line)

        index = 0
        ops = lines[-1]
        total = 0

        while index < len(ops):
            pos = 1 # position of next non-space
            while index + pos < len(ops):
                if (ops[index + pos] == ' '):
                    pos += 1
                else:
                    break

            if (index + pos >= len(ops)):
                length = pos
            else:
                length = pos - 1
        
            sum = 0
            mult = 1

            for l in range(length):
                # sum all lines
                s = ""
                for r in range(len(lines) - 1):
                    s += lines[r][index + l]

                num = int(s)
                if (ops[index] == '+'):
                    sum += num
                elif (ops[index] == '*'):
                    mult *= num
                else:
                    raise ValueError(f"Unknown operation: {ops[index]}")

            if (ops[index] == '+'):
                total += sum
            elif (ops[index] == '*'):
                total += mult
            else:
                raise ValueError(f"Unknown operation: {ops[index]}")
                
            index += length + 1

        return total

    def run(self):
        # filename = f"./Day{self.day}/test{self.day}.txt"
        filename = f"./Day{self.day}/data{self.day}.txt"

        with open(filename, 'r') as file:
            lines = file.readlines()

        numbers = [] # list of list of int
        ops = []    # list of str

        for row in lines:
            row = row.strip()
            parts = row.split(' ')

            numbers_row = []

            for part in parts:
                if not part: continue
                if part.isdigit():
                    numbers_row.append(int(part))
                else:
                    ops.append(part)

            if numbers_row:
                numbers.append(numbers_row)

        # solve_01_result = self.solve_01(numbers, ops)
        # print(solve_01_result)

        solve_02_result = self.solve_02(lines)
        print(solve_02_result)

        pass

'''
4951502530386
Execution time: 0.9 ms

8486156119946
Execution time: 1.9 ms
'''