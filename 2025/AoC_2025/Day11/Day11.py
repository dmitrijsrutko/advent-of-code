class Day11:
    def __init__(self, day: str):
        self.day = day

    def solve_01(self, adjacencyList: dict[str, list[str]]) -> int:
        # do iterative BFS -> count ways each node can be reached at a given stage
        # iterate when new stages appear, count separately

        total = 0
        source = {"you": 1}

        while len(source) > 0:
            dest = {}
            for node in source:
                if node == "out":
                    total += source[node]
                    continue

                for neighbor in adjacencyList[node]:
                    if neighbor in dest:
                        dest[neighbor] += source[node]
                    else:
                        dest[neighbor] = source[node]

            source = dest
            pass

        return total

    def solve_02(self, adjacencyList: dict[str, list[str]]) -> int:

        total = 0
        source = {("svr", False, False): 1}

        while len(source) > 0:
            dest = {}
            for (node, fft, dac), count in source.items():
                if node == "out":
                    if (fft and dac):
                        total += count
                    continue

                for neighbor in adjacencyList[node]:
                    n_fft = (neighbor == "fft")
                    n_dac = (neighbor == "dac")

                    new_state = (neighbor, fft or n_fft, dac or n_dac)

                    if new_state in dest:
                        dest[new_state] += count
                    else:
                        dest[new_state] = count

            source = dest
            pass

        return total

    def run(self):
        # filename = f"./Day{self.day}/test{self.day}.txt"
        # filename = f"./Day{self.day}/test{self.day}_2.txt"
        filename = f"./Day{self.day}/data{self.day}.txt"

        with open(filename, 'r') as file:
            lines = file.readlines()

        adjacencyList = {}

        for row in lines:
            row = row.strip()
            parts = row.replace(':', "").split(' ')
            adjacencyList[parts[0]] = parts[1:]

        # solve_01_result = self.solve_01(adjacencyList)
        # print(solve_01_result)

        solve_02_result = self.solve_02(adjacencyList)
        print(solve_02_result)

        pass

'''
690
Execution time: 0.4 ms

557332758684000
Execution time: 3.5 ms
'''