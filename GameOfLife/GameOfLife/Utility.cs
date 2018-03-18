using System;

using SFML.Graphics;
using SFML.System;

namespace GameOfLife

{
    public static class Utility
    {
        public const string CONSOLE_FONT_PATH = "./Fonts/arial.ttf";

        public static Font consoleFont;

        public static void LoadContent()
        {
            consoleFont = new Font(CONSOLE_FONT_PATH);
            
        }

        public static int Clamp(int min, int max, int current)
        {
            if (current < min) current = min;
            if (current > max) current = max;
            return current;
        }
        public static float Clamp(float min, float max, float current)
        {
            if (current < min) current = min;
            if (current > max) current = max;
            return current;
        }


    }
}
