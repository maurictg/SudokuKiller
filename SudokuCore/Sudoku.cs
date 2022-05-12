using System.Text;
using SudokuCore.Abstractions;

namespace SudokuCore;

public class Sudoku : ICloneable, IDisposable
{
    //private const string CHARS = " 123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    
    public readonly byte[,] Map;
    public List<Region> Regions { get; }
    public byte Size { get; }
    
    public byte BoxWidth { get; }
    public byte BoxHeight { get; }

    /// <summary>
    /// Create sudoku from string format
    /// </summary>
    /// <example>8..........36......7..9.2...5...7.......457.....1...3...1....68..85...1..9....4..</example>
    /// <param name="sudoku">The sudoku string</param>
    /// <returns>The sudoku object</returns>
    public static Sudoku FromString(string sudoku)
    {
        var sq = Math.Sqrt(sudoku.Length);
        if (sq % 1 != 0)
            throw new NotSupportedException(
                "The provided sudoku is not supported. It must be a 3x3, 4x4, 5x5 etc. sudoku");

        var size = (byte) sq;

        sudoku = sudoku.Replace(".", "0");
        
        var s = new Sudoku(size);
        for (byte x = 0; x < size; x++)
        for (byte y = 0; y < size; y++)
        {
            var i = x * size + y;
            s.Map[x, y] = (byte)char.GetNumericValue(sudoku[i]);
        }

        return s;
    }
    
    public Sudoku(byte size = 9)
    {
        Regions = new List<Region>();
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
    {
        if (x >= Size) throw new IndexOutOfRangeException("Position of x does not exist");
        if (y >= Size) throw new IndexOutOfRangeException("Position of y does not exist");
        return new Cell(this, (x, y));
    }

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
        return new Box(this, (y, x)); //for some reason, inverted
    }
    
    public string ToSingleLine()
    {
        var str = new StringBuilder();
        for (var x = 0; x < Size; x++)
        {
            for (var y = 0; y < Size; y++) 
                str.Append($"{Map[x, y]}");
        }

        return str.ToString();
    }

    public override string ToString()
    {
        var sqrt = Math.Sqrt(Size);
        var str = new StringBuilder();
        for (var y = 0; y < Size; y++)
        {
            for (var x = 0; x < Size; x++)
            {
                str.Append($" {Map[y, x]} ");
                if (sqrt % 1 == 0 && (x + 1) % sqrt == 0 && x < Size - 1)
                    str.Append("|");
            }
            
            if (sqrt % 1 == 0 && (y+1) % sqrt == 0 && y < Size - 1)
            {
                str.Append("\n ");
                str.Append(new string('-', Size * 3 + ((int)sqrt-3)));
            }
            
            str.Append('\n');
        }

        return str.ToString();
    }

    public IEnumerable<byte> DomainValues()
    {
        for (byte i = 1; i <= Size; i++)
            yield return i;
    }

    public IEnumerable<Cell> GetCells()
    {
        for (byte x = 0; x < Size; x++)
        for (byte y = 0; y < Size; y++)
        {
            yield return Cell(x, y);
        }
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Sudoku s) return false;
        if (s.Size != Size) return false;
        for (byte x = 0; x < Size; x++)
            for (byte y = 0; y < Size; y++)
                if (s.Cell(x, y).Value != Cell(x, y).Value)
                    return false;
        return true;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Map, Size, BoxWidth, BoxHeight);
    }

    public object Clone()
    {
        return FromString(ToSingleLine());
    }

    public void Dispose()
    {
    }
}