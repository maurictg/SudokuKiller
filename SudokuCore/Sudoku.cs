using System.Text;

namespace SudokuCore;

public class Sudoku
{
    public readonly byte[,] Map;
    public byte Size { get; }
    
    public byte BoxWidth { get; }
    public byte BoxHeight { get; }
    
    public Sudoku(byte size = 9)
    {
        Size = size;
        Map = new byte[Size, Size];

        //Sorry, I needed to know the correlation between the box dimensions and the size
        IEnumerable<(byte, byte)> CalcDivisors()
        {
            var sq = Math.Sqrt(size);

            if (sq % 1 == 0)
            {
                yield return ((byte)sq, (byte)sq);
                yield break;
            }

            //Algorithm to calculate divisors
            for (byte i = 1; i < sq; i++)
            {
                if (size % i == 0)
                {
                    if (size / i == i)
                        yield return (i, i);
                    else
                        yield return (i, (byte)(size / i));
                }
            }
        }

        (BoxHeight, BoxWidth) = CalcDivisors().Last();
    }
    
    public Cell Cell(byte x, byte y)
        => new(this, (x, y));

    public Row Row(Orientation orientation, byte index)
    {
        if (index >= Size)
            throw new IndexOutOfRangeException("Row does not exist");
        
        return new Row(this, index, orientation);
    }

    public Box Box(byte x, byte y)
    {
        if (x > Size / BoxWidth) throw new IndexOutOfRangeException("X is greater than the amount of boxes");
        if (y > Size / BoxHeight) throw new IndexOutOfRangeException("Y is greater than the amount of boxes");
        return new Box(this, (x, y));
    }

    public override string ToString()
    {
        var str = new StringBuilder();
        for (var x = 0; x < Size; x++)
        {
            for (var y = 0; y < Size; y++) 
                str.Append($" {Map[x, y]} ");

            str.Append('\n');
        }

        return str.ToString();
    }
}