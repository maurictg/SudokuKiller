namespace SudokuCore;

public readonly struct Cell
{
    private readonly Sudoku _sudoku;
    public (byte x, byte y) Location { get; }

    public bool IsEmpty => Value == 0;

    public byte Value
    {
        get => _sudoku.Map[Location.x, Location.y];
        set => _sudoku.Map[Location.x, Location.y] = value;
    }

    public Cell(Sudoku sudoku, (byte x, byte y) location)
        => (Location, _sudoku) = (location, sudoku);

    public IEnumerable<Region> GetParentRegions()
    {
        var loc = Location;
        return _sudoku.Regions.Where(x => x.Locations.Contains(loc));
    }
    
    public Row GetParentRow(Orientation orientation)
    {
        return orientation switch
        {
            Orientation.Horizontal => _sudoku.Row(Orientation.Horizontal, Location.x),
            Orientation.Vertical => _sudoku.Row(Orientation.Vertical, Location.y),
            _ => throw new NotImplementedException($"Getting a {orientation} parent row is not supported yet.")
        };
    }

    public Box GetParentBox()
        => _sudoku.Box((byte)Math.Floor((double)Location.x / _sudoku.BoxWidth), (byte)Math.Floor((double)Location.y / _sudoku.BoxHeight));

    public override string ToString()
    {
        return $"{Value} : {Location}";
    }
}