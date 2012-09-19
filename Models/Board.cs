using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicTacToeConsole.Models;

namespace TicTacToeConsole.Models
{
    public class Board
    {
        public string[,] Positions { get; private set; }
        public bool GameActive { get; set; }
        public string BestMove { get; private set; }

        public Board()
        {
            GameActive = true;
            ResetBoard();
        }

        public void ResetBoard()
        {
            Positions = new string[3,3]
                {
                   {"1-1", "1-2", "1-3" },
                   {"2-1", "2-2", "2-3" },
                   {"3-1", "3-2", "3-3" }
                };
        }

        public void UpdateBoardAndCheckForWin(PositionUpdate positionUpdate)
        {
            Positions[positionUpdate.X-1, positionUpdate.Y-1] = 
                positionUpdate.PositionIdentifier.ToString();

            CheckForWinnerAndSetBestMove(positionUpdate.PositionIdentifier.ToString());
        }

        public void CheckForWinnerAndSetBestMove(string currentPlayer)
        {
            var groups = GetWinningGroupLists();
            BestMove = string.Empty;
            if (groups.SelectMany(x => x.FindAll(y => y.Contains("-"))).Any())
            {
                foreach (var group in groups)
                {
                    if (group.Count(x => x.Equals(currentPlayer)) == 3)
                    {
                        GameActive = false;
                        Console.WriteLine(string.Format("{0} wins!", currentPlayer));
                    }
                    else if (group.Count(x => x.Equals(currentPlayer)) == 0
                    && group.FindAll(x => x.Contains("-")).Count == 1)
                    {
                        BestMove = group.Where(x => x.Contains("-"))
                                      .FirstOrDefault().ToString();
                        break;
                    }
                    else if (group.Count(x => x.Equals(currentPlayer)) == 2
                        && group.FindAll(x => x.Contains("-")).Any())
                    {
                        BestMove = group.Where(x => x.Contains("-"))
                                      .FirstOrDefault().ToString();
                    }
                }
                if (string.IsNullOrEmpty(BestMove))
                {
                    if (groups.Where(x => x.FindAll(y => y.Equals("2-2")).Any()).Any())
                    {
                        BestMove = Positions[1, 1];
                    }
                    else
                    {
                        BestMove = groups.SelectMany(x => x.FindAll(y => y.Contains("-")))
                            .FirstOrDefault();
                    }
                }
            }
            else
            {
                GameActive = false;
                Console.WriteLine("It's a tie!");
            }
        }

        public void DrawBoard()
        {
            for (int i = 0; i < 3; i++)
            {
                if (i == 1 || i ==2)
                {
                    Console.WriteLine("________________________");
                }
                Console.WriteLine();
                Console.WriteLine("{0}   |   {1}   |   {2}",
                                    Positions[i, 0],
                                        Positions[i, 1],
                                            Positions[i, 2]);
            }
        }

        private IList<List<string>> GetWinningGroupLists()
        {
            List<List<string>> groups = new List<List<string>>();
            for (int i = 0; i < 3; i++)
            {
                    groups.Add(new List<string>
                    {
                        Positions[i, 0],
                        Positions[i, 1],
                        Positions[i, 2]
                    });
                    groups.Add(new List<string>
                    {
                        Positions[0, i],
                        Positions[1, i],
                        Positions[2, i]
                    });
            }
            groups.Add(new List<string>
                        {
                            Positions[0, 0],
                            Positions[1, 1],
                            Positions[2, 2]
                        });

            groups.Add(new List<string>
                        {
                            Positions[0, 2],
                            Positions[1, 1],
                            Positions[2, 0]
                        });
            return groups;
        }
    }

    public class PositionUpdate
    {
        public PlayerSymbol PositionIdentifier { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
