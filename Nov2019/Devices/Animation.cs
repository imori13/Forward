using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nov2019.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nov2019.Devices
{
    class Animation
    {
        public bool RepeatFlag { get; set; }
        public bool FinishFlag { get; private set; }    // リピートがオフの時、終了したことを知らせるフラグ
        string assetName;
        Vector2 rectSize;
        public Color Color { get; set; }
        public float Radian { get; set; }   // ラジアン角度
        public Vector2 drawSize { get; set; }
        int maxCount;
        float updateTime;
        public GameObject GameObject { get; private set; }

        float time;
        int count;

        public Animation(GameObject gameObject, string assetName, Vector2 rectSize, int maxCount, float updateTime)
        {
            this.assetName = assetName;
            this.rectSize = rectSize;
            this.maxCount = maxCount;
            this.updateTime = updateTime;
            GameObject = gameObject;
            drawSize = Vector2.One;
            count = 0;
            FinishFlag = false;
            RepeatFlag = true;
            Color = Color.White;
        }

        public Animation Clone()
        {
            Animation animation = new Animation(GameObject, assetName, rectSize, maxCount, updateTime);
            animation.RepeatFlag = RepeatFlag;

            return animation;
        }

        public void Initialize()
        {
            count = 0;
            FinishFlag = false;
            RepeatFlag = true;
            Color = Color.White;
        }

        public void Update()
        {
            time += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds * TimeSpeed.Time;

            // 時間がたったら
            if (time >= updateTime)
            {
                // カウント++
                count++;

                // もしカウントがMaxCountよりも小さいなら
                if (count < maxCount)
                {
                    // もう一度時間を数えなおす
                    time = 0;
                }
                // もしカウントがMaxCountよりも超えたら
                else
                {
                    // リピートフラグがtrueなら最初から
                    if (RepeatFlag)
                    {
                        time = 0;
                        count = 0;
                    }
                    // リピートしないなら
                    else
                    {
                        // Getできるフラグをtrueにしておく
                        FinishFlag = true;
                    }
                }
            }
        }

        public void Draw(Renderer renderer, Vector2 offset, SpriteEffects spriteEffects = SpriteEffects.None)
        {
            Rectangle rectangle = new Rectangle(
                (int)rectSize.X * count, 0,
                (int)rectSize.X, (int)rectSize.Y);

            renderer.Draw2D(assetName, GameObject.Position + offset, rectangle, Color, Radian, rectSize / 2f, Vector2.One * drawSize * Screen.ScreenSize, spriteEffects);
        }
    }
}
