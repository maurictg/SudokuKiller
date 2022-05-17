using System.Diagnostics;
using SudokuCore.Abstractions;

namespace SudokuCore.Solvers;

public class OptimizedBacktrackSolver : ISudokuSolver
{
    private readonly Sudoku _sudoku;
    private int _checks;
    private int _backtracks;
    private int _emptySearches;
    private int _forwardCellUpdates;

    public OptimizedBacktrackSolver(Sudoku sudoku)
    {
        _sudoku = sudoku;
    }

    private IEnumerable<Cell> GetRemainingCells(Cell?[,] matrix)
    {
        for (byte x = 0; x < _sudoku.Size; x++)
        for (byte y = 0; y < _sudoku.Size; y++)
        {
            _emptySearches++;
            var c = matrix[x, y];
            if (c != null)
                yield return c.Value;
        }
    }

    private IEnumerable<Cell> FindMrvCells(IEnumerable<Cell> cells)
    {
        //Improvement 1: select cells with Minimum Remaining Values
        var lst = cells.ToList();
        var min = lst.Min(x => x.RemainingValues.Count);
        return lst.Where(x => x.RemainingValues.Count == min);
    }

    private Cell?[,]? Update(Cell?[,] matrix, Cell c, byte value)
    {
        //Deeply clone matrix
        var newMatrix = new Cell?[_sudoku.Size, _sudoku.Size];
        for (byte x = 0; x < _sudoku.Size; x++)
        for (byte y = 0; y < _sudoku.Size; y++)
        {
            if (matrix[x, y].HasValue)
                newMatrix[x, y] = (Cell)matrix[x, y]!.Value.Clone();
        }
        
        var updateCells = c.GetParentBox().Cells()
            .Concat(c.GetParentRow(Orientation.Horizontal).Cells())
            .Concat(c.GetParentRow(Orientation.Vertical).Cells())
            //.Concat(c.GetParentRegions().SelectMany(x => x.Cells()))
            .Select(x => x.Location)
            .Where(x => x != c.Location)
            .Distinct();

        foreach (var (x,y) in updateCells)
        {
            if (newMatrix[x, y].HasValue)
            {
                _forwardCellUpdates++;
                var cell = newMatrix[x, y]!.Value;
               // Console.WriteLine($"{value}@{cell}" + string.Join(',',cell.RemainingValues));
                
                //cell.RemainingValues.Clear();
                //cell.RemainingValues.AddRange(cell.GetPossibleValues());
                cell.RemainingValues.Remove(value);
                
                //For the killer sudoku: remove possibilities that are not allowed by parent regions
                cell.RemainingValues.RemoveAll(num => !cell.GetParentRegions().All(r => r.Accepts(num)));

                //Forward checking: if the change causes another empty cell to get out-of-options, reject this matrix and send null
                if (cell.RemainingValues.Count == 0)
                {
                    newMatrix[x,y] = null;
                    //Console.WriteLine($"This won't lead to an answer for {value} -> {c}!");
                    return null;
                }
            }
        }

        newMatrix[c.Location.x, c.Location.y] = null;
        return newMatrix;
    }

    private bool SolveBacktrack(Cell?[,] matrix)
    {
        //1. Find empty cell
        var emptyCells = GetRemainingCells(matrix).ToList();
        
        //2. If no empty cells, soduku is solved
        if (emptyCells.Count == 0)
            return true;

        //3. Find cells with minimum remaining values
        var mrv_cells = FindMrvCells(emptyCells);
        var cell = mrv_cells.First();
        if (!cell.RemainingValues.Any())
            throw new ArgumentException("Got cell with no remaining values...");
        
        //TODO: select most constraining variable out of these cells

        //4. Try all allowed values (check less)
        foreach (var num in cell.RemainingValues)
        {
            _checks++;
            cell.Value = num;

            //5. Try to solve the sudoku with the new value and new matrix recursively
            var newMatrix = Update(matrix, cell, num);
            
            //5a. If matrix is null (means forward checking failed), go to next option
            if (newMatrix != null)
            {
                //5b. else, check recursively with backtracking if the new matrix will lead to a solution
                if (SolveBacktrack(newMatrix))
                    return true;
            }
            
            //If sudoku can't be solved recursively this way, set cell back to empty (backtracking)
            cell.Value = 0;
            _backtracks++;
        }
        
        //All possibilities tried, but no solution found
        return false;
    }

    public void Solve()
    {
        //0. Preparation: create matrix
        var matrix = new Cell?[_sudoku.Size,_sudoku.Size];
        var cnt = 0;
        foreach (var c in _sudoku.GetCells().Where(x => x.IsEmpty))
        {
            var cell = c;
            cell.RemainingValues.AddRange(c.GetPossibleValues());
            matrix[c.Location.x, c.Location.y] = cell;
            cnt++;
        }
        
        Console.WriteLine($"Prepared matrix with {cnt} empty cells");
        
        var sw = Stopwatch.StartNew();
        if(!SolveBacktrack(matrix))
            throw new ArgumentException("Sudoku could not be solved: No solution exists");

        Console.WriteLine($"Found BackTrack-optimized solution in {sw.Elapsed} - {sw.ElapsedMilliseconds}ms and {_checks} checks + {_forwardCellUpdates} forward-updates with {_backtracks} backtracks and {_emptySearches} empty checks");
    }
}