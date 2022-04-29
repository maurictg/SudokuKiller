namespace SudokuCore;

public static class SudokuRules
{
    public static bool IsValidAndUnique(ICellCollection row, bool strict = true)
    {
        var values = row.Cells().Select(x => x.Value).ToList();
        return (!strict || values.All(x => x != 0)) && values.Distinct().Count() == values.Count;
    }

    public static bool CheckIfSudokuValid(Sudoku sudoku, bool strict = true)
    {
        //Check horizontal rows
        for (byte i = 0; i < sudoku.Size; i++)
        {
            if (!IsValidAndUnique(sudoku.Row(Orientation.Horizontal, i), strict))
                return false;
        }
        
        //Check vertical rows
        for (byte i = 0; i < sudoku.Size; i++)
        {
            if (!IsValidAndUnique(sudoku.Row(Orientation.Vertical, i), strict))
                return false;
        }
        
        //Check boxes
        for (byte x = 0; x < sudoku.Size / sudoku.BoxWidth; x++)
            for (byte y = 0; y < sudoku.Size / sudoku.BoxHeight; y++)
                if (!IsValidAndUnique(sudoku.Box(x,y), strict))
                    return false;

        //Otherwise, correct!
        return true;
    }
}