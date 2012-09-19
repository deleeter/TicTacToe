using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicTacToeConsole.Models
{
    public class Player
    {
        public string Name { get; set; }
        public int Wins { get; set; }
    }
    public enum PlayerSymbol
    {
        X, O
    }
}
