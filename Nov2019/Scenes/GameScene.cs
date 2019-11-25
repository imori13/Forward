using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nov2019.Devices;
using Nov2019.Devices.Particles;
using Nov2019.GameObjects;
using Nov2019.SceneDevices;
using Nov2019.ScenesDevice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nov2019.Scenes
{
    class GameScene : SceneBase
    {
        Camera Camera;
        BossEnemy BossEnemy;
        Player Player;
        ObjectsManager ObjectsManager;
        FrameCounter frameCounter;

        float inputTime;
        static readonly float inputLImit = 1f;
        float height;
        bool endingFlag;
        float endingTime;
        float textAlpha;
        static readonly float endingLimit = 2;
        Color color;

        public GameScene()
        {
            Camera = new Camera();
            ObjectsManager = new ObjectsManager(Camera);
            Player = new Player();
            BossEnemy = new BossEnemy();
            frameCounter = new FrameCounter();

            Camera.ObjectsManager = ObjectsManager;
        }

        public override void Initialize()
        {
            Time.Initialize();

            Camera.Initialize();

            ObjectsManager.Initialize();

            float distance = ObjectsManager.MapLength / 2f;

            for (int i = 0; i < 5000f; i++)
            {
                ObjectsManager.AddGameObject(new Star(new Vector3(MyMath.RandF(-distance, distance), MyMath.RandF(-500, 500), MyMath.RandF(-distance, distance))), true);
            }

            ObjectsManager.AddGameObject(Player, true);
            ObjectsManager.AddGameObject(BossEnemy, true);

            height = 0;
            inputTime = 0;
            endingFlag = false;
            endingTime = 0;
            textAlpha = 1;
            color = new Color(100, 100, 100);

            base.Initialize();
        }

        public override void Update()
        {
            Camera.Update(Player);

            ObjectsManager.Update();

            frameCounter.Update(GameDevice.Instance().GameTime);

            if (BossEnemy.GAMECLAER_FLAG || Player.DeathFlag)
            {
                if (Input.GetKey(Keys.Space))
                {
                    inputTime += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds;
                    if (inputTime >= inputLImit)
                    {
                        endingFlag = true;
                    }
                }
                else
                {
                    inputTime = MathHelper.Lerp(inputTime, 0, 0.1f * Time.deltaNormalSpeed);
                }
            }

            if (endingFlag)
            {
                endingTime += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds;
                if (endingTime >= endingLimit)
                {
                    IsEndFlag = true;
                }

                color = Color.Lerp(color, new Color(50, 50, 50), 0.1f * Time.deltaNormalSpeed);
                textAlpha = MathHelper.Lerp(textAlpha, 0, 0.1f * Time.deltaNormalSpeed);
            }
            else
            {
                height = Easing2D.ExpInOut(inputTime, inputLImit, 0, Screen.HEIGHT);
            }

            base.Update();
        }

        public override void Draw(Renderer renderer)
        {
            renderer.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            ObjectsManager.Draw(renderer);

            renderer.Begin2D();
            ObjectsManager.DrawUI(renderer);

            SpriteFont font = Fonts.FontCica_32;
            string text;
            Vector2 size;

            if (MyDebug.DebugMode)
            {
                text = "オブジェクト数 : " + ObjectsManager.objectCount;
                size = font.MeasureString(text);
                renderer.DrawString(font, text, new Vector2(Screen.WIDTH, Screen.HEIGHT - size.Y * 0f * Screen.ScreenSize), Color.White, 0, new Vector2(size.X, size.Y), Vector2.One * Screen.ScreenSize);

                text = "パーティクル数 : " + ObjectsManager.particleCount;
                size = font.MeasureString(text);
                renderer.DrawString(font, text, new Vector2(Screen.WIDTH, Screen.HEIGHT - size.Y * 1f * Screen.ScreenSize), Color.White, 0, new Vector2(size.X, size.Y), Vector2.One * Screen.ScreenSize);

                text = "FPS値 : " + frameCounter.FPS.ToString("0.00").PadLeft(3, ' ');
                size = font.MeasureString(text);
                renderer.DrawString(font, text, new Vector2(Screen.WIDTH, Screen.HEIGHT - size.Y * 2f * Screen.ScreenSize), Color.White, 0, new Vector2(size.X, size.Y), Vector2.One * Screen.ScreenSize);
            }

            renderer.Draw2D("Pixel", new Vector2(Screen.WIDTH / 2f, Screen.HEIGHT), color, 0, new Vector2(0.5f, 1), new Vector2(Screen.WIDTH, height));

            // ゲームクリアUI
            if ((BossEnemy.GAMECLAER_FLAG || Player.DeathFlag) && !Time.TimeStopMode)
            {
                if (ObjectsManager.BossEnemy.GAMECLAER_FLAG)
                {
                    renderer.Draw2D("GAMECLEAR_TEXT", Screen.Vec2 / 2f - Vector2.UnitY * 100 * Screen.ScreenSize, Color.White * textAlpha, 0, new Vector2(282.5f, 30), Vector2.One * 1.5f * Screen.ScreenSize);
                    renderer.Draw2D("THANKYOU_FOR_PLAYING_TEXT", Screen.Vec2 / 2f, Color.White * textAlpha, 0, new Vector2(282.5f, 15), Vector2.One * Screen.ScreenSize);
                }
                else
                {
                    renderer.Draw2D("GAMEOVER_TEXT", Screen.Vec2 / 2f - Vector2.UnitY * 100 * Screen.ScreenSize, Color.White * textAlpha, 0, new Vector2(240, 30), Vector2.One * 1.5f * Screen.ScreenSize);
                }
                renderer.Draw2D("SPACEHOLD_TO_RETURN_TEXT", Screen.Vec2 / 2f + Vector2.UnitY * 200 * Screen.ScreenSize, Color.White * textAlpha, 0, new Vector2(330, 35), Vector2.One * 0.5f * Screen.ScreenSize);
            }


            renderer.End();

            base.Draw(renderer);
        }

        public override SceneEnum NextScene()
        {
            return SceneEnum.GameScene;
        }
    }
}
