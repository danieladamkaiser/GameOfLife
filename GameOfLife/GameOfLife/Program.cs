using System;

using SFML.Graphics;

namespace GameOfLife
{
    class Program
    {
        private RenderWindow window { get; set; }

        static void Main(string[] args)
        {
            GameOfLife gameOfLife = new GameOfLife();
            gameOfLife.Run();
        }
    }
}
