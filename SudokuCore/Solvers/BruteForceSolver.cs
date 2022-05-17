using System.Diagnostics;
using SudokuCore.Abstractions;

namespace SudokuCore.Solvers;

public class BruteForceSolver : ISudokuSolver
{
    private readonly Sudoku _sudoku;
    private int _checks;

    public BruteForceSolver(Sudoku sudoku)
    {
        _sudoku = sudoku;
    }

    private bool SolveCell(byte x, byte y)
    {
        _checks++;
        if (x == _sudoku.Size && y == _sudoku.Size - 1)
            return true; //stop recursion if at last cell
        
        //If at end of column, go to next column
        if (x == _sudoku.Size)
        {
            y++;
            x = 0;
        }

        var cell = _sudoku.Cell(x, y);

        //If cell already set, check next cell
        if (!cell.IsEmpty)
            return SolveCell((byte)(x + 1), y);

        //Check each value in domain for the variable/cell (1 ... Size)
        for (byte i = 1; i <= _sudoku.Size; i++)
        {
            if (cell.Accepts(i)) //if value fits, set it
            {
                cell.Value = i;
                //Recursively check if the value could lead to an outcome
                if (SolveCell((byte)(x + 1), y))
                    return true;
            }
            
            //could not hold/does not lead to outcome from recursion, reset it to empty and try another value
            cell.Value = 0;
        }

        return false;
    }
    
    public void Solve()
    {
        var sw = Stopwatch.StartNew();
        if (!SolveCell(0, 0))
            throw new ArgumentException("Sudoku could not be solved: No solution exists");
        
        Console.WriteLine($"Found BruteForce solution in {sw.Elapsed} - {sw.ElapsedMilliseconds}ms and {_checks} tries");
    }
}