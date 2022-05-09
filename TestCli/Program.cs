using SudokuCore;
using SudokuCore.Solvers;

var sudoku = Sudoku.FromString("8..........36......7..9.2...5...7.......457.....1...3...1....68..85...1..9....4..");
var sudoku2 = (Sudoku)sudoku.Clone();
Console.WriteLine("Input: \n"+sudoku);

var bts = new BacktrackSolver(sudoku);
bts.Solve();

Console.WriteLine(sudoku.ToString());
Console.WriteLine(sudoku.ToSingleLine());
Console.WriteLine(SudokuRules.CheckIfSudokuValid(sudoku));


var bfs = new BruteForceSolver(sudoku2);
bfs.Solve();
Console.WriteLine(sudoku2);
Console.WriteLine(sudoku2.ToSingleLine());
Console.WriteLine(SudokuRules.CheckIfSudokuValid(sudoku2));

if (!Equals(sudoku, sudoku2))
    throw new ArgumentException("Solvers solved it different! ALARM!");

//if (sudoku.ToSingleLine() != "692573148158624379734891265417265983325948716986317524561789432273456891849132657")
//    throw new ArgumentException("Solver offered different solution. ALARM!");

// 8..........36......7..9.2...5...7.......457.....1...3...1....68..85...1..9....4..
// 812753649943682175675491283154237896369845721287169534521974368438526917796318452