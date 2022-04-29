namespace SudokuCore;

public class Box : ICellCollection
{
    private readonly Sudoku _sudoku;
    public (byte x, byte y) Location { get; }
    
    public Box(Sudoku sudoku, (byte x, byte y) location)
        => (Location, _sudoku) = (location, sudoku);
    
    public IEnumerable<Cell> Cells()
    {
        var x = (byte)(Location.x * _sudoku.BoxWidth);
        var y = (byte)(Location.y * _sudoku.BoxHeight);

        var nx = x + _sudoku.BoxWidth;
        var ny = y + _sudoku.BoxHeight;

        for (var i = y; i < ny; i++)
            for (var j = x; j < nx; j++)
                yield return _sudoku.Cell(i, j);
    }
}