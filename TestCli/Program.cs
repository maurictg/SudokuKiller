using SudokuCore;
using SudokuCore.Solvers;

/*
var killer = new Sudoku();
Console.WriteLine(killer);


killer.Regions.Add(new KillerRegion(killer, 15, (0,0), (0,1)));
killer.Regions.Add(new KillerRegion(killer, 16, (0,2), (1,0), (1,1), (1,2)));
killer.Regions.Add(new KillerRegion(killer, 14, (2,0), (2,1), (2,2)));
killer.Regions.Add(new KillerRegion(killer, 25, (0,3), (1,3), (1,4), (2,4), (2,5)));
killer.Regions.Add(new KillerRegion(killer, 15, (0,4), (0,5), (1,5)));
killer.Regions.Add(new KillerRegion(killer, 11, (2,3), (3,3)));
killer.Regions.Add(new KillerRegion(killer, 20, (0,6), (0,7), (1,7)));
killer.Regions.Add(new KillerRegion(killer, 9, (0,8), (1,8)));
killer.Regions.Add(new KillerRegion(killer, 15, (1,6), (2,6), (2,7)));
killer.Regions.Add(new KillerRegion(killer, 9, (2,8), (3,8)));
killer.Regions.Add(new KillerRegion(killer, 9, (3,0), (4,0), (4,1)));
killer.Regions.Add(new KillerRegion(killer, 11, (3,1), (3,2)));
killer.Regions.Add(new KillerRegion(killer, 18, (5,0), (5,1), (5,2)));
killer.Regions.Add(new KillerRegion(killer, 16, (4,2), (4,3)));
killer.Regions.Add(new KillerRegion(killer,20, (3,4), (3,5), (4,4)));
killer.Regions.Add(new KillerRegion(killer, 5, (5,3), (5,4)));
killer.Regions.Add(new KillerRegion(killer, 5, (4,5), (5,5)));
killer.Regions.Add(new KillerRegion(killer, 10, (4,6), (5,6)));
killer.Regions.Add(new KillerRegion(killer, 27, (3,6), (3,7), (4,7), (4,8), (5,7), (5,8)));
killer.Regions.Add(new KillerRegion(killer, 27, (6,0), (6,1), (6,2), (6,3), (7,0)));
killer.Regions.Add(new KillerRegion(killer, 26, (7,1), (7,2), (8,0), (8,1), (8,2)));
killer.Regions.Add(new KillerRegion(killer, 11, (7,3), (8,3)));
killer.Regions.Add(new KillerRegion(killer, 26, (6,4), (6,5), (7,4), (7,5), (8,4), (8,5)));
killer.Regions.Add(new KillerRegion(killer, 31, (6,6), (6,7), (6,8), (7,8), (8,7), (8,8)));
killer.Regions.Add(new KillerRegion(killer, 14, (7,6), (7,7), (8,6)));

var bts = new BruteForceSolver(killer);
bts.Solve();

Console.WriteLine(killer);*/

var sudoku = Sudoku.FromString("8..........36......7..9.2...5...7.......457.....1...3...1....68..85...1..9....4..");

var bfs = new BacktrackWithForwardCheckSolver(sudoku);

bfs.Solve();

Console.WriteLine(sudoku);


// 8..........36......7..9.2...5...7.......457.....1...3...1....68..85...1..9....4..
// 812753649943682175675491283154237896369845721287169534521974368438526917796318452*/

//.61..7..3.92..3..............853..........5.45....8....4......1...16.8..6........

// Not solvable w/ backtracking or brute-force:
//..............3.85..1.2.......5.7.....4...1...9.......5......73..2.1........4...9
