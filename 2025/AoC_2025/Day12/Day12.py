class Day12:
    def __init__(self, day: str):
        self.day = day

    def run(self):
        # filename = f"./Day{self.day}/test{self.day}.txt"
        filename = f"./Day{self.day}/data{self.day}.txt"

        with open(filename, 'r') as file:
            lines = file.readlines()

        sizes = []

        for i in range(6):
            s = lines[i * 5 + 1] + lines[i * 5 + 2] + lines[i * 5 + 3]
            weight = s.count('#')
            sizes.append(weight)
            pass
        
        count = 0
        
        for line in lines:
            line = line.strip().replace("x", " ").replace(":", "")
            parts = line.split(" ")
            if (len(parts) == 8):
                width = int(parts[0])
                length = int(parts[1])

                sum = 0
                for s in parts[2:]:
                    size = int(s)
                    sum += size

                w3 = (width // 3)
                l3 = (length // 3)

                square = w3 * l3
                fit = sum <= square

                result = ""
                if fit:
                    result = "True"
                    count += 1

                weight = 0
                for i in range(len(parts) - 2):
                    size = int(parts[i + 2])
                    weight += sizes[i] * size

                max_weight = w3 * l3 * 3 * 3

                if (weight > max_weight):
                    result = "False"

                if (result == ""):
                    raise ValueError("Should not happen")

                pass


        print(count)

        pass

'''
427
Execution time: 1.7 ms
'''