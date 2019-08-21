using System;
using Klopodavka.Game;

namespace Klopodavka.BlazorUi
{
    class Program
    {
        static void Main(string[] args)
        {
            var p = new Player("Player 1", 1);
            Console.WriteLine(p);
        }
    }
}
