using System.Diagnostics;
using SudokuCore.Abstractions;

namespace SudokuCore.Solvers;

public class BacktrackSolver : ISudokuSolver
{
    private readonly Sudoku _sudoku;
    private int _emptyChecks;
    private int _checks;
    private int _backtracks;

    public BacktrackSolver(Sudoku sudoku)
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

        //Try all values in domain
        foreach (var num in _sudoku.DomainValues())
        {
            _checks++;
            
            //If value is possible, set value
            if (cell.Accepts(num))
            {
                cell.Value = num;
                
                //Try to solve the sudoku with the new value recursively
                if (SolveBacktrack())
                    return true;
                
                //If sudoku can't be solved recursively this way, set cell back to empty
                cell.Value = 0;
                _backtracks++;
            }
        }
        
        //All possibilities tried, but no solution found
        return false;
    }

    public void Solve()
    {
        var sw = Stopwatch.StartNew();
        if(!SolveBacktrack())
            throw new ArgumentException("Sudoku could not be solved: No solution exists");

        Console.WriteLine($"Found BackTrack solution in {sw.Elapsed} - {sw.ElapsedMilliseconds}ms and {_checks} checks + {_emptyChecks} empty checks, with {_backtracks} backtracks");
    }
}