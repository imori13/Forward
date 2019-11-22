using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nov2019.Devices
{
    static class Time
    {
        static float time;
        static float dest;
        public static float deltaTime { get { return time * frame; } private set { time = value; } }
        public static float deltaSpeed { get { return time * frame * 60; } }
        static float frame;
        public static bool StopTimeMode { get; private set; }
        public static float stopTime { get; private set; }
        public static readonly float stopLimit = 8;

        public static void Initialize()
        {
            time = 1;
            StopTimeMode = false;
        }

        public static void Update()
        {
            dest = (Input.GetKey(Keys.T)) ? (0.2f) : (1.00f);

            if (StopTimeMode)
            {
                stopTime += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds;

                dest = 0.025f;

                if (stopTime >= stopLimit)
                {
                    stopTime = 0;
                    StopTimeMode = false;

                    GameDevice.Instance().Sound.ResumeBGM();
                }
            }

            // 1フレームにかかった時間
            // ほとんどの移動、時間の変化に関係する処理にTimeSpeed.Timeがかけられているので、
            // TimeSpeed.Timeにフレーム秒が影響されるようにする。
            frame = (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds;

            time = MathHelper.Lerp(time, dest, 0.05f);
        }

        public static void Draw(Renderer renderer)
        {
            renderer.Draw2D("slowMode", Vector2.Zero, Color.White * (1 - time), 0, Vector2.Zero, Vector2.One * Screen.ScreenSize * 1.2f);
        }

        public static void StopTime()
        {
            StopTimeMode = true;
        }
    }
}
