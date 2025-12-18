import time

def is_accesible(row: int, col: int, grid: list[str]) -> bool:
    if grid[row][col] != '@':
        return False

    # check all 8 directions
    directions = [
        (-1, -1), (-1, 0), (-1, +1),
        (0, -1),           (0, +1),
        (+1, -1), (+1, 0), (+1, +1)
    ]    

    rolls = 0    

    for (dr, dc) in directions:
        (r, c) = (row + dr, col + dc)
        if 0 <= r < len(grid) and 0 <= c < len(grid[0]) and grid[r][c] == '@':
            rolls += 1
            
    return (rolls < 4)

def solve_01(grid: list[str]) -> int:

    # iterate over integer positions in the grid
    accessible = 0
    for row in range(len(grid)):
        for col in range(len(grid[row])):
            if is_accesible(row, col, grid):
                print("x", end="")
                accessible += 1
            else:
                print(grid[row][col], end="")
        print()

    return accessible

def solve_02(grid: list[list]) -> int:

    is_print = False
    accessible = 0

    changed = True
    while changed:
        # iterate over integer positions in the grid
        changed = False
        for row in range(len(grid)):
            for col in range(len(grid[row])):
                if is_accesible(row, col, grid):
                    if is_print: print("x", end="")
                    accessible += 1
                    grid[row][col] = '.'
                    changed = True
                else:
                    if is_print: print(grid[row][col], end="")
            if is_print: print()
        if is_print: print()

    return accessible

def main():
    # filename = "test04.txt"
    filename = "data04.txt"

    grid = []   # array of lines

    with open(filename, "r") as file:
        lines = file.readlines()

    for line in lines:
        grid.append(list(line.strip()))

    start = time.time()

    # result_01 = solve_01(grid)
    # print(result_01)

    result_02 = solve_02(grid)
    print(result_02)

    elapsed_ms = (time.time() - start) * 1000
    print(f"Execution time: {elapsed_ms:.1f} ms")

    pass

if __name__ == "__main__":
    main()

'''
1433
Execution time: 14.8 ms

8616
Execution time: 183.5 ms
'''