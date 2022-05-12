using System.Diagnostics;
using SudokuCore.Abstractions;

namespace SudokuCore.Solvers;

public class BacktrackWithForwardCheckSolver : ISudokuSolver
{
    private readonly Sudoku _sudoku;
    private int _emptyChecks;
    private int _checks;
    private int _backtracks;
    private int _forwardChecks;

    public BacktrackWithForwardCheckSolver(Sudoku sudoku)
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

    private bool ForwardCheck(Dictionary<Cell, List<byte>> remainingValues, byte value, Cell c)
    {
        //Check if removed value is not the only possibility for another cell in the same row or col
        var cellsToCheck = c.GetParentBox().Cells()
            .Concat(c.GetParentRow(Orientation.Horizontal).Cells())
            .Concat(c.GetParentRow(Orientation.Vertical).Cells())
            .Where(x => x.IsEmpty && !Equals(x, c));

        foreach (var ctc in cellsToCheck)
        {
            _forwardChecks++;
            var possib = remainingValues[ctc];
            if (possib.Count == 1 && possib.Contains(value))
                return false;
        }

        return true;
    }

    private bool SolveBacktrack()
    {
        //1. Find empty cell
        var nextCell = FindEmptyCell();
        
        //2. If no empty cells, soduku is solved
        if (nextCell == null)
            return true;

        var cell = nextCell.Value;
        
        //3. Get remaining values map
        var remainingValues = _sudoku.GetCells().Where(x => x.IsEmpty)
            .ToDictionary(x => x, y => y.GetPossibleValues().ToList());

        var values = remainingValues[cell];
        foreach (var val in values)
        {
            _checks++;
            //4. Check if chosen value isn't the only option for another cell
            if (ForwardCheck(remainingValues, val, cell))
            {
                cell.Value = val;
                if (SolveBacktrack())
                    return true;

                cell.Value = 0;
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

        Console.WriteLine($"Found BackTrack solution in {sw.Elapsed} and {_checks} checks + {_emptyChecks} empty checks, with {_backtracks} backtracks and {_forwardChecks} cells forward checked");
    }
}