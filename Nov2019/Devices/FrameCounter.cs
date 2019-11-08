using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nov2019.Devices
{
    class FrameCounter
    {
        public double FPS { get; private set; }

        double interval;
        int counter;
        double timer;

        public FrameCounter()
        {
            interval = 0.1;
        }

        public void Update(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.TotalSeconds;
            counter++;

            while (timer > interval)
            {
                FPS = counter / timer;

                counter = 0;
                timer -= interval;
            }
        }
    }
}
