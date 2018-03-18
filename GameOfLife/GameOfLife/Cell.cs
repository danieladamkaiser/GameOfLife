using System;
using System.Collections.Generic;

using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace GameOfLife
{
    public class Cell
    {
        public int activeNeighbours;
        public bool alive = false;
        public int x { get; private set; }
        public int y { get; private set; }


        public Cell(int x,int y)
        {
            this.x = x;
            this.y = y;
        }


        public bool liveLongAndProsper()
        {
            if (activeNeighbours < 2 || activeNeighbours > 3) alive=false;
            if ( activeNeighbours == 3) alive = true;
            activeNeighbours = 0;
            return alive;
        }



    }
}
