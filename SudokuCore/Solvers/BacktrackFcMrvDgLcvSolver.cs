using System.Diagnostics;
using SudokuCore.Abstractions;

namespace SudokuCore.Solvers;

public class BacktrackFcMrvDgLcvSolver : ISudokuSolver
{
    private readonly Sudoku _sudoku;
    private int _checks;
    private int _backtracks;
    private int _forwardChecks;

    public BacktrackFcMrvDgLcvSolver(Sudoku sudoku)
    {
        _sudoku = sudoku;
    }

    private bool ForwardCheck(Dictionary<Cell, List<byte>> remainingValues, byte value, List<Cell> cellsToCheck)
    {
        //Check if removed value is not the only possibility for another cell in the same row or col
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
        //1. Get remaining values map
        var remainingValues = _sudoku.GetCells().Where(x => x.IsEmpty)
            .ToDictionary(x => x, y => y.GetPossibleValues().ToList());

        //2. If no empty cells, soduku is solved
        if (!remainingValues.Any())
            return true;
        
        //3. Find the cells with the minimal remaining value
        var mrv = remainingValues.Min(x => x.Value.Count);
        var constrainedCells = remainingValues.Where(x => x.Value.Count == mrv)
            .Select(x => x.Key).ToList();

        //4. Find most constraining variable if multiple cells with least remaining value
        var nextCell = constrainedCells.Count == 1 
            ? constrainedCells.First() 
            : constrainedCells.Select(x => (x, GetDegree(x))).MinBy(x => x.Item2).x;

        var relatedCells = nextCell.GetParentBox().Cells()
            .Concat(nextCell.GetParentRow(Orientation.Horizontal).Cells())
            .Concat(nextCell.GetParentRow(Orientation.Vertical).Cells())
            .Distinct()
            .Where(x => x.IsEmpty && !Equals(x, nextCell))
            .ToList();

        var values = nextCell.GetPossibleValues().ToList();

        while (values.Any())
        {
            //5. Find least constraining value
            var value = GetLcv(remainingValues, values, relatedCells);
            values.Remove(value);

            _checks++;

            if (ForwardCheck(remainingValues, value, relatedCells))
            {
                nextCell.Value = value;
                if (SolveBacktrack())
                    return true;

                nextCell.Value = 0;
                _backtracks++;
            }
        }

        return false;
    }

    
    private byte GetLcv(Dictionary<Cell, List<byte>> remainingValues, List<byte> values, List<Cell> relatedCells)
    {
        //Find least constraining value
        var lcvs = new Dictionary<byte, int>();

        foreach (var val in values)
        {
            var cnt = 0;
            
            foreach (var ctc in relatedCells)
            {
                if (remainingValues[ctc].Contains(val))
                {
                    cnt++;
                }
            }
            
            lcvs.Add(val, cnt);
        }

        return lcvs.MinBy(x => x.Value).Key;
    }

    private int GetDegree(Cell cell)
    {
        //return the number of variables constrained by the cell
        return cell.GetParentBox()
            .Cells()
            .Concat(cell.GetParentRow(Orientation.Horizontal).Cells())
            .Concat(cell.GetParentRow(Orientation.Vertical).Cells())
            .Distinct()
            .Count(x => x.IsEmpty && !Equals(x, cell));
    }

    public void Solve()
    {
        var sw = Stopwatch.StartNew();
        if(!SolveBacktrack())
            throw new ArgumentException("Sudoku could not be solved: No solution exists");

        Console.WriteLine($"Found BackTrack/ForwardCheck/MinimumRemainingValue/Degree/LeastConstrainingValue solution in {sw.Elapsed} and {_checks} checks, with {_backtracks} backtracks and {_forwardChecks} cells forward checked");
    }
}