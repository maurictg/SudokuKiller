namespace SudokuCore.Solvers.DancingLinks;

public class Dlx
{
    private readonly NodeHeader _root;
    private readonly List<NodeHeader> _columns;
    private readonly List<Node> _rows;
    private readonly Stack<Node> _solutionNodes;
    private int _initial;

    public Dlx(int rows, int cols)
    {
        _columns = new List<NodeHeader>(cols);
        _rows = new List<Node>(rows);
        _solutionNodes = new Stack<Node>();
        _root = new NodeHeader();
    }
    
    public void Give(int row) {
        _solutionNodes.Push(_rows[row]);
        CoverMatrix(_rows[row]);
        _initial++;
    }
 
    public IEnumerable<int[]> Solutions() {
        try {
            var node = ChooseSmallestColumn().Down;
            do {
                if (node == node.Header) {
                    if (node == _root) {
                        yield return _solutionNodes.Select(n => n.Row).ToArray();
                    }
                    
                    if (_solutionNodes.Count > _initial) {
                        node = _solutionNodes.Pop();
                        UncoverMatrix(node);
                        node = node.Down;
                    }
                } else {
                    _solutionNodes.Push(node);
                    CoverMatrix(node);
                    node = ChooseSmallestColumn().Down;
                }
            } while(_solutionNodes.Count > _initial || node != node.Header);
        } finally {
            Restore();
        }
    }
    
    private void Restore() {
        while (_solutionNodes.Count > 0) 
            UncoverMatrix(_solutionNodes.Pop());
        _initial = 0;
    }
 
    private NodeHeader ChooseSmallestColumn() {
        NodeHeader traveller = _root, choice = _root;
        do {
            traveller = (NodeHeader)traveller.Right;
            if (traveller.Size < choice.Size) choice = traveller;
        } while (traveller != _root && choice.Size > 0);
        return choice;
    }

    public void AddHeader()
    {
        var h = new NodeHeader(_root.Left, _root);
        h.Attach(Direction.Left | Direction.Right);
        _columns.Add(h);
    }

    public void AddRow(params int[] row)
    {
        Node? first = null;
        foreach (var t in row)
        {
            if (t < 0) continue;
            if (first == null) 
                first = AddNode(_rows.Count, t);
            else AddNode(first, t);
        }
        
        _rows.Add(first!);
    }
    
    private Node AddNode(int row, int column) {
        var n = new Node(row, null,null, _columns[column].Up, _columns[column], _columns[column]);
        n.Attach(Direction.Up | Direction.Down);
        n.Header.Size++;
        return n;
    }
 
    private void AddNode(Node firstNode, int column) {
        var n = new Node(firstNode.Row, firstNode.Left, firstNode, _columns[column].Up, _columns[column], _columns[column]);
        n.Attach(Direction.Left | Direction.Right | Direction.Up | Direction.Down);
        n.Header.Size++;
    }
    
    private void CoverRow(Node row) {
        var traveller = row.Right;
        while (traveller != row) {
            traveller.Detach(Direction.Up | Direction.Down);
            traveller.Header.Size--;
            traveller = traveller.Right;
        }
    }
 
    private void UncoverRow(Node row) {
        var traveller = row.Left;
        while (traveller != row) {
            traveller.Attach(Direction.Up | Direction.Down);
            traveller.Header.Size++;
            traveller = traveller.Left;
        }
    }
 
    private void CoverColumn(Node column) {
        column.Detach(Direction.Left | Direction.Right);
        var traveller = column.Down;
        while (traveller != column) {
            CoverRow(traveller);
            traveller = traveller.Down;
        }
    }
 
    private void UncoverColumn(Node column) {
        var traveller = column.Up;
        while (traveller != column) {
            UncoverRow(traveller);
            traveller = traveller.Up;
        }
        column.Attach(Direction.Left | Direction.Right);
    }
 
    private void CoverMatrix(Node node) {
        var traveller = node;
        do {
            CoverColumn(traveller.Header);
            traveller = traveller.Right;
        } while (traveller != node);
    }
 
    private void UncoverMatrix(Node node) {
        var traveller = node;
        do {
            traveller = traveller.Left;
            UncoverColumn(traveller.Header);
        } while (traveller != node);
    }

}