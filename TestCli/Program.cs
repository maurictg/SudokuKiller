using SudokuCore;
using SudokuCore.Solvers;
using SudokuCore.Variants;


var killer = new Sudoku();
Console.WriteLine(killer);

killer.Regions.Add(new KillerRegion(killer, 3, (0,0), (0,1)));
killer.Regions.Add(new KillerRegion(killer, 15, (0,2), (0,3), (0,4)));
killer.Regions.Add(new KillerRegion(killer, 22, (0,5), (1,4), (1,5), (2,4)));
killer.Regions.Add(new KillerRegion(killer, 4, (0,6), (1,6)));
killer.Regions.Add(new KillerRegion(killer, 16, (0,7), (1,7)));
killer.Regions.Add(new KillerRegion(killer, 15, (0,8), (1,8), (2,8), (3,8)));
killer.Regions.Add(new KillerRegion(killer, 25, (1,0), (1,1), (2,0), (2,1)));
killer.Regions.Add(new KillerRegion(killer, 17, (1,2), (1,3)));
killer.Regions.Add(new KillerRegion(killer, 9, (2,2), (2,3), (3,3)));
killer.Regions.Add(new KillerRegion(killer, 8, (2,5), (3,5), (4,5)));
killer.Regions.Add(new KillerRegion(killer, 20, (2,6), (2,7), (3,6)));
killer.Regions.Add(new KillerRegion(killer, 6, (3,0), (4,0)));
killer.Regions.Add(new KillerRegion(killer, 14, (3,1), (3,2)));
killer.Regions.Add(new KillerRegion(killer, 17, (3,4), (4,4), (5,4)));
killer.Regions.Add(new KillerRegion(killer, 17, (3,7), (4,7), (4,6)));
killer.Regions.Add(new KillerRegion(killer, 13, (4,1), (4,2), (5,1)));
killer.Regions.Add(new KillerRegion(killer, 20, (4,3), (5,3), (6,3)));
killer.Regions.Add(new KillerRegion(killer, 12, (4,8), (5,8)));
killer.Regions.Add(new KillerRegion(killer, 27, (5,0), (6,0), (7,0), (8,0)));
killer.Regions.Add(new KillerRegion(killer, 6, (5,2), (6,2), (6,1)));
killer.Regions.Add(new KillerRegion(killer, 20, (5,5), (6,5), (6,6)));
killer.Regions.Add(new KillerRegion(killer, 6, (5,6), (5,7)));
killer.Regions.Add(new KillerRegion(killer, 10, (6,4), (7,3), (7,4), (8,3)));
killer.Regions.Add(new KillerRegion(killer, 14, (6,7), (6,8), (7,7), (7,8)));
killer.Regions.Add(new KillerRegion(killer, 8, (7,1), (8,1)));
killer.Regions.Add(new KillerRegion(killer, 16, (7,2), (8,2)));
killer.Regions.Add(new KillerRegion(killer, 15, (7,5), (7,6)));
killer.Regions.Add(new KillerRegion(killer, 13, (8,4), (8,5), (8,6)));
killer.Regions.Add(new KillerRegion(killer, 17, (8,7), (8,8)));

Console.WriteLine("Hello!");

var sudoku = Sudoku.FromString("8..........36......7..9.2...5...7.......457.....1...3...1....68..85...1..9....4..");
var sudoku2 = Sudoku.FromString(sudoku.ToSingleLine());
var bts = new OptimizedBacktrackSolver(sudoku);
bts.Solve();

Console.WriteLine(sudoku);

var bts2 = new BacktrackSolver(sudoku2);
bts2.Solve();
Console.WriteLine(sudoku2);

// 8..........36......7..9.2...5...7.......457.....1...3...1....68..85...1..9....4..
// 812753649943682175675491283154237896369845721287169534521974368438526917796318452*/

//.61..7..3.92..3..............853..........5.45....8....4......1...16.8..6........

// Not solvable w/ backtracking or brute-force:
//..............3.85..1.2.......5.7.....4...1...9.......5......73..2.1........4...9
