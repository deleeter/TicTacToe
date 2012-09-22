using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicTacToeConsole.Models;
using System.Threading;

namespace TicTacToeConsole
{
    public class Program
    {
        private static Board _board;

        static void Main(string[] args)
        {
            BeginPlaying();
        }
        private static void BeginPlaying()
        {
            _board = new Board();
            GetPlayerInformation();
            GetPlayerMove();
            _board.DrawBoard();
            BeginPlaying();
        }

        private static void GetPlayerMove()
        {
            while (_board.GameActive)
            {
                Console.WriteLine("Please select a position. ");
                _board.DrawBoard();
                string response = Console.ReadLine();
                if (response.ToUpper() == "QUIT")
                    System.Environment.Exit(0);

                if (UpdateBoard(response))
                {
                    Console.WriteLine("Thinking...");
                    Thread.Sleep(1);
                    if (_board.GameActive)
                        UpdateBoard(_board.BestMove, true);
                }
            }
        }

        private static bool UpdateBoard(string move, bool computer = false)
        {
            int x = 0;
            int y = 0;
            PlayerSymbol currentPlayer = computer
                ? PlayerSymbol.O
                : PlayerSymbol.X;

            if (!string.IsNullOrEmpty(move) 
                && _board.HasAvailablePosition(move))
            {
                string[] coordinateArray = move.Split('-');
                Int32.TryParse(coordinateArray[0], out x);
                Int32.TryParse(coordinateArray[1], out y);
                if (x > 0 && y > 0)
                {
                    _board.UpdateBoardAndCheckForWin(new PositionUpdate
                    {
                        PositionIdentifier = currentPlayer,
                        X = x,
                        Y = y
                    });
                }
                else
                {
                    ShowInvalidInputError();
                    return false;
                }
            }
            else
            {
                ShowInvalidInputError();
                return false;
            }
            return true;
        }

        private static void ShowInvalidInputError()
        {
            Console.WriteLine("Invalid input.");
            Console.Write("Please enter your move in x-y integer format. \n");
        }

        private static void GetPlayerInformation()
        {
            Console.WriteLine("\n\n\nWhat is your name?" );
            string name = Console.ReadLine();
            if (!string.IsNullOrEmpty(name))
            {
                Console.WriteLine(
                    string.Format("Thanks! Let's get going, {0}!", name));

                Console.WriteLine("Type quit to exit.");
            }
            else
            {
                Console.WriteLine("Please enter your name again! ");
            }
        }
    }
}
