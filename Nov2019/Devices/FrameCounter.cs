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
            // フレーム数を増やす（目標は１秒に６０回）
            counter++;

            // タイマーに前のフレームから過ぎた時間を加算する
            timer += gameTime.ElapsedGameTime.TotalSeconds;

            // タイマーが1秒を超えたら
            if (timer > interval)
            {
                // FPSを計算する, 速度が下がっていた場合はここで差を計算する
                FPS = counter / timer;

                // タイマーとカウンターをリセットする
                counter = 0;
                timer -= interval;
            }
        }
    }
}
