using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace GameOfLife
{
    public abstract class GameLoop
    {

        public const int TARGET_FPS = 60;
        public const float TIME_UNTIL_UPDATE = 1f / TARGET_FPS;

        public RenderWindow window
        {
            get;
            protected set;
        }

        public GameTime gameTime
        {
            get;
            protected set;
        }
            
        public Color windowColor
        {
            get;
            protected set;
        }

        protected GameLoop(uint windowWidth,uint windowHeight, string windowTitle, Color windowColor)
        {
            this.windowColor = windowColor;
            this.window = new RenderWindow(new VideoMode(windowWidth, windowHeight), windowTitle, Styles.Titlebar);
            this.gameTime = new GameTime();
            window.Closed += Window_Closed;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            window.Close();
        }

        public void Run()
        {
            loadContent();
            initialize();

            float timeBeforeUpdate = 0f;
            float previousTimeElapsed = 0f;
            float deltaTime = 0f;
            float totalTime = 0f;

            Clock clock = new Clock();

            while (window.IsOpen)
            {
                window.DispatchEvents();

                totalTime = clock.ElapsedTime.AsSeconds();
                deltaTime = totalTime - previousTimeElapsed;
                previousTimeElapsed = totalTime;

                timeBeforeUpdate += deltaTime;

                if(timeBeforeUpdate>=TIME_UNTIL_UPDATE)
                {
                    gameTime.Update(timeBeforeUpdate, clock.ElapsedTime.AsSeconds());
                    timeBeforeUpdate = 0;

                    update(gameTime);
                    window.Clear(windowColor);
                    draw(gameTime);
                    window.Display();
                }
            }
        }

        public abstract void loadContent();
        public abstract void initialize();
        public abstract void update(GameTime gameTime);
        public abstract void draw(GameTime gameTime);
    }


}
