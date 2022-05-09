using SudokuCore.Abstractions;

namespace SudokuCore;

public static class SudokuRules
{
    public static bool IsValidAndUnique(ICellCollection coll, bool strict = true)
    {
        var values = coll.Cells().Select(x => x.Value).ToList();
        return (!strict || values.All(x => x != 0)) && values.Where(x => x != 0).Distinct().Count() == values.Count(x => x != 0);
    }

    public static bool CheckIfSudokuValid(Sudoku sudoku, bool strict = true)
    {
        //Check horizontal rows
        for (byte i = 0; i < sudoku.Size; i++)
        {
            if (!IsValidAndUnique(sudoku.Row(Orientation.Horizontal, i), strict))
                return false;
            //Console.WriteLine($"Horizontal row {i} correct");
        }
        
        //Check vertical rows
        for (byte i = 0; i < sudoku.Size; i++)
        {
            if (!IsValidAndUnique(sudoku.Row(Orientation.Vertical, i), strict))
                return false;
            //Console.WriteLine($"Vertical row {i} correct");
        }
        
        //Check boxes
        for (byte x = 0; x < sudoku.Size / sudoku.BoxWidth; x++)
            for (byte y = 0; y < sudoku.Size / sudoku.BoxHeight; y++)
                if (!IsValidAndUnique(sudoku.Box(x,y), strict))
                    return false;
                //else
                    //Console.WriteLine($"Box {x},{y} correct");

        //Otherwise, correct!
        return true;
    }
}