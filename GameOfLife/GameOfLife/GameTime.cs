using System;

namespace GameOfLife
{
    public class GameTime
    {
        private float deltaTime = 0f;
        private float timeScale = 1f;

        public float TimeScale
        {
            get { return timeScale; }
            set { timeScale = value; }
        }

        public float DeltaTime
        {
            get { return deltaTime * timeScale; }
            private set { deltaTime = value; }
        }

        public float deltaTimeUnscaled
        {
            get { return deltaTime; }
        }

        public float timeElapsed
        {
            get;
            private set;
        }

        public GameTime()
        {

        }

        public void Update(float deltaTime, float timeElapsed)
        {
            this.deltaTime = deltaTime;
            this.timeElapsed = timeElapsed;
        }

    }
}
