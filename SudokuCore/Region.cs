using SudokuCore.Abstractions;

namespace SudokuCore;

public abstract class Region : ICellCollection
{
    private readonly Sudoku _sudoku;
    public List<(byte x, byte y)> Locations { get; }
    
    public Region(Sudoku sudoku, params (byte x, byte y)[] locations)
    {
        _sudoku = sudoku;
        Locations = locations.ToList();
    }

    public IEnumerable<Cell> Cells() 
        => Locations.Select(l => new Cell(_sudoku, l));

    public abstract bool Accepts(byte value);
}