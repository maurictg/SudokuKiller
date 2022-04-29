namespace SudokuCore;

public readonly struct Cell
{
    private readonly Sudoku _sudoku;
    public (byte x, byte y) Location { get; }

    public byte Value
    {
        get => _sudoku.Map[Location.x, Location.y];
        set => _sudoku.Map[Location.x, Location.y] = value;
    }

    public Cell(Sudoku sudoku, (byte x, byte y) location)
        => (Location, _sudoku) = (location, sudoku);
    
    public Row GetParentRow(Orientation orientation)
    {
        return orientation switch
        {
            Orientation.Horizontal => _sudoku.Row(Orientation.Horizontal, Location.x),
            Orientation.Vertical => _sudoku.Row(Orientation.Vertical, Location.y),
            _ => throw new NotImplementedException($"Getting a {orientation} parent row is not supported yet.")
        };
    }

    public override string ToString()
    {
        return $"{Value} : {Location}";
    }
}