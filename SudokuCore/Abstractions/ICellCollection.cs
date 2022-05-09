namespace SudokuCore.Abstractions;

public interface ICellCollection
{
    public IEnumerable<Cell> Cells();
    public bool Accepts(byte value);
}