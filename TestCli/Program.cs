using SudokuCore;

var sudoku = new Sudoku(4)
{
    Map =
    {
        [0, 0] = 2,
        [0, 1] = 4,
        [0, 2] = 3,
        [0, 3] = 1,
        [1, 0] = 3,
        [1, 1] = 1,
        [1, 2] = 4,
        [1, 3] = 2,
        [2, 0] = 1,
        [2, 1] = 3,
        [2, 2] = 2,
        [2, 3] = 4,
        [3, 0] = 4,
        [3, 1] = 2,
        [3, 2] = 1,
        [3, 3] = 3
    }
};

Console.WriteLine(SudokuRules.CheckIfSudokuValid(sudoku, true));