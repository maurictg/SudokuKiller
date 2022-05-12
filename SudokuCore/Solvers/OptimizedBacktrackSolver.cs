using System.Diagnostics;
using SudokuCore.Abstractions;

namespace SudokuCore.Solvers;

public class OptimizedBacktrackSolver : ISudokuSolver
{
    private readonly Sudoku _sudoku;
    private int _emptyChecks;
    private int _checks;
    private int _backtracks;

    public OptimizedBacktrackSolver(Sudoku sudoku)
    {
        _sudoku = sudoku;
    }
    
    private Cell? FindEmptyCell()
    {
        foreach (var cell in _sudoku.GetCells())
        {
            _emptyChecks++;
            if(cell.IsEmpty)
                return cell;
        }
        
        return null;
    }

    private bool SolveBacktrack()
    {
        //1. Find empty cell
        var nextCell = FindEmptyCell();
        
        //2. If no empty cells, soduku is solved
        if (nextCell == null)
            return true;

        var cell = nextCell.Value;

        //3. Try all allowed values (check less)
        foreach (var num in cell.GetPossibleValues())
        {
            _checks++;
            
            cell.Value = num;
            //Try to solve the sudoku with the new value recursively
            if (SolveBacktrack())
                return true;
                
            //If sudoku can't be solved recursively this way, set cell back to empty
            cell.Value = 0;
            _backtracks++;
        }
        
        //All possibilities tried, but no solution found
        return false;
    }

    public void Solve()
    {
        var sw = Stopwatch.StartNew();
        if(!SolveBacktrack())
            throw new ArgumentException("Sudoku could not be solved: No solution exists");

        Console.WriteLine($"Found BackTrack-optimized solution in {sw.Elapsed} and {_checks} checks + {_emptyChecks} empty checks, with {_backtracks} backtracks");
    }
}