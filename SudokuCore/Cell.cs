namespace SudokuCore;

public struct Cell : ICloneable
{
    private readonly Sudoku _sudoku;
    public (byte x, byte y) Location { get; }

    public bool IsEmpty => Value == 0;
    public List<byte> RemainingValues { get; }

    public byte Value
    {
        get => _sudoku.Map[Location.x, Location.y];
        set => _sudoku.Map[Location.x, Location.y] = value;
    }

    public Cell(Sudoku sudoku, (byte x, byte y) location)
        => (Location, _sudoku, RemainingValues) = (location, sudoku, new List<byte>(sudoku.Size));

    public bool Accepts(byte value)
    {
        return Value == value || (GetParentRow(Orientation.Horizontal).Accepts(value)
            && GetParentRow(Orientation.Vertical).Accepts(value)
            && GetParentBox().Accepts(value)
            && GetParentRegions().All(x => x.Accepts(value)));
    }

    public IEnumerable<byte> GetPossibleValues()
    {
        foreach (var v in _sudoku.DomainValues())
            if (Accepts(v))
                yield return v;
    }

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
        => _sudoku.Box((byte)Math.Floor((double)Location.x / _sudoku.BoxWidth), 
            (byte)Math.Floor((double)Location.y / _sudoku.BoxHeight));

    public override string ToString()
    {
        return $"{Value} : {Location}";
    }

    public object Clone()
    {
        var clone = new Cell(_sudoku, Location);
        clone.RemainingValues.AddRange(RemainingValues);
        return clone;
    }

    public override int GetHashCode() => Location.x ^ Location.y;
}