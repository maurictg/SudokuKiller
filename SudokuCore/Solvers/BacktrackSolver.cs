using System.Diagnostics;
using SudokuCore.Abstractions;

namespace SudokuCore.Solvers;

public class BacktrackSolver : ISudokuSolver
{
    private readonly Sudoku _sudoku;
    private int _emptyChecks;
    private int _checks;

    public BacktrackSolver(Sudoku sudoku)
    {
        _sudoku = sudoku;
    }

    private bool SolveBacktrack()
    {
        byte xRes = 0;
        byte yRes = 0;
        var empty = true;
        
        for (byte x = 0; x < _sudoku.Size; x++)
        {
            for (byte y = 0; y < _sudoku.Size; y++)
            {
                _emptyChecks++;
                if (_sudoku.Cell(x, y).IsEmpty)
                {
                    xRes = x;
                    yRes = y;
                    empty = false;
                    break;
                }
            }
            
            if (!empty)
                break;
        }

        if (empty)
            return true;
        
        //Backtrack all rows/columns
        for (byte num = 1; num <= _sudoku.Size; num++)
        {
            var cell = _sudoku.Cell(xRes, yRes);
            _checks++;
            if (_sudoku.Accepts(cell, num))
            {
                cell.Value = num;
                if (SolveBacktrack())
                    return true;
                
                cell.Value = 0;
            }
        }
        return false;
    }

    public void Solve()
    {
        var sw = Stopwatch.StartNew();
        if(!SolveBacktrack())
            throw new ArgumentException("Sudoku could not be solved: No solution exists");

        Console.WriteLine($"Found BackTrack solution in {sw.Elapsed} and {_checks} checks + {_emptyChecks} empty checks");
    }
}