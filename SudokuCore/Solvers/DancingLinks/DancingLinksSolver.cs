using System.Diagnostics;
using System.Text;
using SudokuCore.Abstractions;

namespace SudokuCore.Solvers.DancingLinks;

/// <summary>
/// DLX, Backtracking approach with Dancing Links and Algorithm X
/// </summary>
public class DancingLinksSolver : ISudokuSolver
{
    private readonly Sudoku _sudoku;
    private readonly Dlx _dlx;

    public DancingLinksSolver(Sudoku sudoku)
    {
        _sudoku = sudoku;
        
        //Build DLX matrix
        const int rows = 9 * 9 * 9, columns = 4 * 9 * 9;
        _dlx = new Dlx(rows, columns);
        for (var i = 0; i < columns; i++) _dlx.AddHeader();
 
        for (int cell = 0, row = 0; row < 9; row++) {
            for (var column = 0; column < 9; column++) {
                var box = row / 3 * 3 + column / 3;
                for (var digit = 0; digit < 9; digit++) {
                    _dlx.AddRow(cell, 81 + row * 9 + digit, 2 * 81 + column * 9 + digit, 3 * 81 + box * 9 + digit);
                }
                cell++;
            }
        }
    }
    
    public void Solve()
    {
        var puzzle = _sudoku.ToSingleLine();
        for (var i = 0; i < puzzle.Length; i++) {
            if (puzzle[i] == '0' || puzzle[i] == '.') continue;
            if (puzzle[i] < '1' && puzzle[i] > '9') throw new ArgumentException($"Input contains an invalid character: ({puzzle[i]})");
            var digit = puzzle[i] - '0' - 1;
            _dlx.Give(i * 9 + digit);
        }
        
        var sb = new StringBuilder(new string('.', 81));
        
        var sw = Stopwatch.StartNew();
        var solution = _dlx.Solutions().First();
        Console.WriteLine($"Found DLX solution in {sw.Elapsed}");

        foreach (var s in solution)
        {
            sb[s / 81 * 9 + s / 9 % 9] = (char)(s % 9 + '1');
        }

        var sol = Sudoku.FromString(sb.ToString());
        foreach (var c in sol.GetCells())
        { 
            var cell = _sudoku.Cell(c.Location.x, c.Location.y);
            cell.Value = c.Value;
        }
    }
}