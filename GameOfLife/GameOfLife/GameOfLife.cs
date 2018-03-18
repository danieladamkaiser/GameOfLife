using System;
using System.Collections.Generic;
using System.Linq;

using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace GameOfLife
{
    public class GameOfLife : GameLoop
    {

        

        public const uint screenWidth = 1200;
        public const uint screenHeight = 900;


        private float zoom =1;
        private int cellsGridSizeX = 500;
        private int cellsGridSizeY = 500;

        private int offX;
        private int offY;


        private uint horizCellsNumb;
        private uint vertCellsNumb;


        private Cell[,] Cells;
        private Vector2f baseSize = new Vector2f(18, 18);
        private Vector2f size = new Vector2f(18, 18);


        private float baseSpace = 2;
        private float space =2;

        private RectangleShape rectangleWhite;
        private RectangleShape rectangleYellow;

        private Vector2i mousePos;
        private Cell selectedCell;

        public GameOfLife() : base(screenWidth, screenHeight, "Game Of Life", Color.Black)
        { }



        public override void initialize()
        {
            horizCellsNumb = screenWidth / (uint)(size.X+space);
            vertCellsNumb = screenHeight / (uint)(size.Y+space);


            offX = cellsGridSizeX / 2;
            offY = cellsGridSizeY / 2;
            rectangleWhite = new RectangleShape();
            rectangleYellow = new RectangleShape();

            rectangleYellow.Size = rectangleWhite.Size = size;

            rectangleWhite.FillColor = Color.White;
            rectangleYellow.FillColor = Color.Yellow;

            Cells = new Cell[cellsGridSizeX, cellsGridSizeY];


            for (int i = 0; i < cellsGridSizeX; i++)
            {
                for (int j = 0; j < cellsGridSizeY; j++)
                {
                    Cells[i, j] = new Cell(i, j);
                }
            }
        }

        public override void loadContent()
        {
            Utility.LoadContent();
        }

        public override void update(GameTime gameTime)
        {
            trackMouseMove();
            putAndRemoveNewCells();

            playCellsLives();
            controlMap();

        }

        public override void draw(GameTime gameTime)
        {
            drawCells();
        }

        private void drawCells()
        {

            for (int i = 0; i < horizCellsNumb; i++)
            {
                if (i + offX < Cells.GetLength(0))
                {
                    for (int j = 0; j < vertCellsNumb; j++)
                    {
                        if (j + offY < Cells.GetLength(1))
                        {

                            if (Cells[i + offX, j + offY].alive)
                            {
                                rectangleYellow.Position = new Vector2f(i * (size.X + space) + 1, j * (size.Y + space) + 1);
                                window.Draw(rectangleYellow);
                            }
                            else
                            {
                                rectangleWhite.Position = new Vector2f(i * (size.X + space)+ 1, j * (size.Y + space)  + 1);
                                window.Draw(rectangleWhite);
                            }
                        }
                    }
                }
            }
        }


        private void trackMouseMove()
        {
            mousePos = Mouse.GetPosition(window);
            selectedCell = Cells[Utility.Clamp(0, cellsGridSizeX - 1, (int)(mousePos.X / (size.X + space) + offX)), Utility.Clamp(0, cellsGridSizeY - 1, (int)(mousePos.Y / (size.Y + space) + offY))];
        }
        private void putAndRemoveNewCells()
        {
            if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                if (!selectedCell.alive)
                {
                    selectedCell.alive = true;
                    checkNeighbours(selectedCell.x, selectedCell.y);
                }
            }

            if (Mouse.IsButtonPressed(Mouse.Button.Right))
            {
                if (selectedCell.alive)
                {

                    unCheckNeighbours(selectedCell.x, selectedCell.y);
                    selectedCell.alive = false;
                }
            }
        }
        private void playCellsLives()
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
            {
                foreach (Cell cell in Cells)
                {
                    cell.liveLongAndProsper();
                }

                foreach (Cell cell in Cells)
                {
                    checkNeighbours(cell.x, cell.y);
                }

            }
        }
        private void controlMap()
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.R)) foreach (Cell cell in Cells) { cell.alive = false; cell.activeNeighbours = 0; }
            if ((Keyboard.IsKeyPressed(Keyboard.Key.Up)) && offY > 0) offY--;
            if ((Keyboard.IsKeyPressed(Keyboard.Key.Down)) && offY < (cellsGridSizeY - vertCellsNumb)) offY++;
            if ((Keyboard.IsKeyPressed(Keyboard.Key.Left)) && offX > 0) offX--;
            if ((Keyboard.IsKeyPressed(Keyboard.Key.Right)) && offX < (cellsGridSizeX - horizCellsNumb)) offX++;
            if (Keyboard.IsKeyPressed(Keyboard.Key.Escape)) window.Close();

            zoomMap();
        }


        private void zoomMap()
        {

            if (Keyboard.IsKeyPressed(Keyboard.Key.Add))
            {
                zoom += -0.05f;
                zoom = Utility.Clamp(0.25f, 2f, zoom);

                size = baseSize * zoom;
                space = baseSpace * zoom;

                rectangleYellow.Size = rectangleWhite.Size = size;

                horizCellsNumb = screenWidth / (uint)(size.X + space);
                vertCellsNumb = screenHeight / (uint)(size.Y + space);
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.Subtract))
            {
                zoom += 0.05f;
                zoom = Utility.Clamp(0.25f, 2f, zoom);

                size = baseSize * zoom;
                space = baseSpace * zoom;

                rectangleYellow.Size = rectangleWhite.Size = size;

                horizCellsNumb = screenWidth / (uint)(size.X + space);
                vertCellsNumb = (screenHeight / (uint)(size.Y + space));
            }
        }

        private void checkNeighbours(int X, int Y)
        {
            if (Cells[X, Y].alive)
            {
                if (X > 0)
                {
                    if (Y > 0) Cells[X - 1, Y - 1].activeNeighbours++;
                    Cells[X - 1, Y + 0].activeNeighbours++;
                    if (Y < Cells.GetLength(1) - 1) Cells[X - 1, Y + 1].activeNeighbours++;
                }

                if (Y > 0) Cells[X, Y - 1].activeNeighbours++;
                if (Y < Cells.GetLength(1) - 1) Cells[X, Y + 1].activeNeighbours++;

                if (X < Cells.GetLength(0) - 1)
                {
                    if (Y > 0) Cells[X + 1, Y - 1].activeNeighbours++;
                    Cells[X + 1, Y + 0].activeNeighbours++;
                    if (Y < Cells.GetLength(1) - 1) Cells[X + 1, Y + 1].activeNeighbours++;
                }
            }
        }
        private void unCheckNeighbours(int X, int Y)
        {
            if (Cells[X, Y].alive)
            {
                if (X > 0)
                {
                    if (Y > 0) Cells[X - 1, Y - 1].activeNeighbours--;
                    Cells[X - 1, Y + 0].activeNeighbours--;
                    if (Y < Cells.GetLength(1) - 1) Cells[X - 1, Y + 1].activeNeighbours--;
                }

                if (Y > 0) Cells[X, Y - 1].activeNeighbours--;
                if (Y < Cells.GetLength(1) - 1) Cells[X, Y + 1].activeNeighbours--;

                if (X < Cells.GetLength(0) - 1)
                {
                    if (Y > 0) Cells[X + 1, Y - 1].activeNeighbours--;
                    Cells[X + 1, Y + 0].activeNeighbours--;
                    if (Y < Cells.GetLength(1) - 1) Cells[X + 1, Y + 1].activeNeighbours--;
                }
            }
        }
    }

    
}
