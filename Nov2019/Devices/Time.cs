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
        public static float deltaNormalSpeed { get { return frame * 60; } }
        static float frame;
        public static bool TimeStopMode { get; private set; }
        public static float timeStopTime { get; private set; }
        public static readonly float timeStopLimit = 8;
        public static bool HitStopMode { get; private set; }
        public static float hitStopTime { get; private set; }
        public static readonly float hitStioLimit = 1.5f;
        public static bool BossBreakStopMode { get; private set; }
        public static float bossBreakStopTime { get; private set; }
        public static readonly float bossBreakStopLimit = 4f;

        public static void Initialize()
        {
            time = 1;
            TimeStopMode = false;
        }

        public static void Update()
        {
            dest = (Input.GetKey(Keys.T)) ? (0.2f) : (1.00f);

            if (TimeStopMode)
            {
                timeStopTime += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds;

                dest = 0.025f;

                if (timeStopTime >= timeStopLimit)
                {
                    timeStopTime = 0;
                    TimeStopMode = false;

                    GameDevice.Instance().Sound.ResumeBGM();
                }
            }

            if (HitStopMode)
            {
                if (TimeStopMode)
                {
                    hitStopTime = 0;
                    HitStopMode = false;
                }
                else
                {
                    hitStopTime += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds;

                    dest = 0.01f;

                    if (hitStopTime >= hitStioLimit)
                    {
                        hitStopTime = 0;
                        HitStopMode = false;

                        GameDevice.Instance().Sound.ResumeBGM();
                    }
                }
            }

            if (BossBreakStopMode)
            {
                if (TimeStopMode || HitStopMode)
                {
                    bossBreakStopTime = 0;
                    BossBreakStopMode = false;
                }
                else
                {
                    bossBreakStopTime += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds;

                    dest = 0.01f;

                    if (bossBreakStopTime >= bossBreakStopLimit)
                    {
                        BossBreakStopMode = false;
                        bossBreakStopTime = 0;

                        GameDevice.Instance().Sound.ResumeBGM();
                    }
                }
            }

            dest = (MyDebug.DebugTimeStopMode) ? (0) : (dest);

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

        public static void TimeStop()
        {
            TimeStopMode = true;
        }

        public static void HitStop()
        {
            HitStopMode = true;
            time = 0.01f;
        }

        public static void BossBreakStop()
        {
            BossBreakStopMode = true;
            time = 0.01f;
        }
    }
}
