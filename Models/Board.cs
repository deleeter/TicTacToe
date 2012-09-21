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
                string.Format("{0}", positionUpdate.PositionIdentifier.ToString());

            CheckForWinnerAndSetBestMove(positionUpdate.PositionIdentifier.ToString());
        }

        public void CheckForWinnerAndSetBestMove(string currentPlayer)
        {
            var groups = GetWinningGroupLists();
            BestMove = string.Empty;
            if (groups.SelectMany(x => x.FindAll(y => y.IsAvailable())).Any())
            {
                CheckForWinAndBlock(groups, currentPlayer);
                if (string.IsNullOrEmpty(BestMove))
                {
                    BestMove = GetInitialBestMove(groups);
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

        private void CheckForWinAndBlock(IList<List<string>> groups,
            string currentPlayer)
        {
            foreach (var group in groups)
            {
                if (group.Count(x => x.Equals(currentPlayer)) == 3)
                {
                    GameActive = false;
                    Console.WriteLine(
                        string.Format("{0} wins!", currentPlayer));
                    break;
                }
                else if (group.Count(x => x.Equals(currentPlayer)) == 2
                && group.FindAll(x => x.IsAvailable()).Count == 1)
                {
                    BestMove = group.Where(x => x.IsAvailable())
                                  .FirstOrDefault().ToString();
                }
            }
        }

        private string GetInitialBestMove(IList<List<string>> groups)
        {
            string move = GetStrategicMove();
            if (string.IsNullOrEmpty(move))
            {
                move = GetRandomMove();
            }
            
            return move;
        }

        private string GetStrategicMove()
        {
            string move = "";
            if (Positions[1, 1].IsAvailable())
            {
                move = Positions[1, 1];
            }
            else if((Positions[1,1].IsOpponent()
                && (Positions[0, 0].IsOpponent()
                || (Positions[0, 2].IsOpponent()
                || Positions[2, 2].IsOpponent()))))
            {
                move = GetCornerMove();
            }
            else if (Positions[0, 0].IsOpponent()
                && Positions[2, 2].IsOpponent())
            {
                move = GetMiddleMove();
            }
            else if (Positions[0, 2].IsOpponent()
                && Positions[2, 0].IsOpponent())
            {
                move = GetMiddleMove();
            }
            else
            {
                move = GetRandomMove();
            }
            
            return move;
        }

        private string GetCornerMove()
        {
            string move = "";
            if (Positions[0, 0].IsAvailable())
            {
                move = Positions[0, 0];
            }
            else if (Positions[2, 0].IsAvailable())
            {
                move = Positions[2, 0];
            }
            else if (Positions[2, 2].IsAvailable())
            {
                move = Positions[2, 2];
            }
            else if (Positions[0, 2].IsAvailable())
            {
                move = Positions[0, 2];
            }
            return move;
        }

        private string GetMiddleMove()
        {
            string move = "";
            if (Positions[0, 1].IsAvailable())
            {
                move = Positions[0, 1];
            }
            else if (Positions[1, 0].IsAvailable())
            {
                move = Positions[1, 0];
            }
            else if (Positions[1, 2].IsAvailable())
            {
                move = Positions[1, 2];
            }
            return move;
        }


        private string GetRandomMove()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (Positions[i, j].IsAvailable())
                    {
                        return Positions[i, j];
                    }
                }
            }
            return "";
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

    public static class Helpers
    {
        public static bool IsAvailable(this string position)
        {
            return position.Contains("-");
        }

        public static bool IsOpponent(this string position)
        {
            return position.Contains("X");
        }
    }
}
