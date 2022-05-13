namespace SudokuCore.Solvers.DancingLinks;

[Flags]
public enum Direction
{
    Left = 1,
    Right = 2,
    Up = 4,
    Down = 8
}

public class NodeHeader : Node
{
    public NodeHeader(Node? left = null, Node? right = null) : base(-1, left, right) { }
    
    public int Size { get; set; }
}

public class Node
{
    public Node(int row, Node? left = null, Node? right = null, Node? up = null, Node? down = null, NodeHeader? head = null) {
        Left = left ?? this;
        Right = right ?? this;
        Up = up ?? this;
        Down = down ?? this;
        Header = head ?? (NodeHeader)this;
        Row = row;
    }
    
    public NodeHeader Header { get; set; }
    public int Row { get; set; }
    
    public Node Left { get; set; }
    public Node Right { get; set; }
    public Node Up { get; set; }
    public Node Down { get; set; }
    

    public void Attach(Direction directions)
    {
        if ((directions & Direction.Left) != 0) Left.Right = this;  // (left)->[me]
        if ((directions & Direction.Right) != 0) Right.Left = this; // [me]<-(right)
        if ((directions & Direction.Up) != 0) Up.Down = this;       //(up)->[me]
        if ((directions & Direction.Down) != 0) Down.Up = this;     //[me]<-(down)
    }
    
    public void Detach(Direction directions)
    {
        if ((directions & Direction.Left) != 0) Left.Right = Right; // (left)->(right) (left) x [me]
        if ((directions & Direction.Right) != 0) Right.Left = Left; // (left)<-(right) [me] x (right)
        if ((directions & Direction.Up) != 0) Up.Down = Down;       // (up)->(down) (up) x [me]
        if ((directions & Direction.Down) != 0) Down.Up = Up;       // (up)<-(down) [me] x (down)
    }
}