using SudokuCore.Abstractions;

namespace SudokuCore;

public readonly struct Row : ICellCollection
{
    private readonly Sudoku _sudoku;
    private readonly (byte idx, Orientation o) _location;
    
    public Row(Sudoku sudoku, byte index, Orientation orientation)
    {
        _sudoku = sudoku;
        _location = (index, orientation);
    }

    public IEnumerable<Cell> Cells()
    {
        switch (_location.o)
        {
            case Orientation.Horizontal:
            case Orientation.Vertical:
                for (byte i = 0; i < _sudoku.Size; i++)
                {
                    if (_location.o == Orientation.Horizontal)
                        yield return _sudoku.Cell(_location.idx, i);
                    else
                        yield return _sudoku.Cell(i, _location.idx);
                }
                break;
            default:
                throw new NotSupportedException($"Iteration of {_location.o} rows is not supported");
        }
    }

    public bool Accepts(byte value)
        => Cells().All(c => c.Value != value);

    public override string ToString()
    {
        return $"[{string.Join(" ", Cells().Select(x => x.Value))}]";
    }
}