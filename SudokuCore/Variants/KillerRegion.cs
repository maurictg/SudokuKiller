namespace SudokuCore.Variants;

public class KillerRegion : Region
{
    private readonly int _sum;

    public KillerRegion(Sudoku sudoku, int sum, params (byte x, byte y)[] locations) : base(sudoku, locations)
    {
        _sum = sum;
    }

    public override bool Accepts(byte value)
    {
        var fields = Cells().Select(x => x.Value).ToList();
        if (fields.Contains(value))
            return false;
            
        var expected = fields.Sum(x => x) + value;
        var placesLeft = fields.Count(x => x == 0);

        return ((placesLeft > 1 && expected <= _sum) || (placesLeft == 1 && expected == _sum));
    }
}