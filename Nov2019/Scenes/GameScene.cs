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
        Player Player;
        ObjectsManager ObjectsManager;
        FrameCounter frameCounter;

        public GameScene()
        {
            Camera = new Camera();
            ObjectsManager = new ObjectsManager(Camera);
            Player = new Player();
            frameCounter = new FrameCounter();
        }

        public override void Initialize()
        {
            ObjectsManager.AddGameObject(Player, true);

            base.Initialize();
        }

        public override void Update()
        {
            Camera.Update(Player);

            ObjectsManager.Update();

            if (Input.GetKeyDown(Keys.G))
            {
                for (int i = 0; i < 100f; i++)
                {
                    ObjectsManager.AddParticle(new Spark_Particle3D(Vector3.Zero, MyMath.RandomCircleVec3(), GameDevice.Instance().Random));
                }
            }

            frameCounter.Update(GameDevice.Instance().GameTime);

            base.Update();
        }

        public override void Draw(Renderer renderer)
        {
            renderer.Begin();
            ObjectsManager.Draw(renderer);
            renderer.End();

            renderer.Begin();
            ObjectsManager.DrawUI(renderer);

            SpriteFont font = Fonts.FontCica_32;
            string text;
            Vector2 size;

            text = "オブジェクト数 : " + ObjectsManager.objectCount;
            size = font.MeasureString(text);
            renderer.DrawString(font, text, new Vector2(Screen.WIDTH, Screen.HEIGHT - size.Y * 0f), Color.White, 0, new Vector2(size.X, size.Y), Vector2.One);

            text = "パーティクル数 : " + ObjectsManager.particleCount;
            size = font.MeasureString(text);
            renderer.DrawString(font, text, new Vector2(Screen.WIDTH, Screen.HEIGHT - size.Y * 1f), Color.White, 0, new Vector2(size.X, size.Y), Vector2.One);

            text = "FPS値 : " + frameCounter.FPS;
            size = font.MeasureString(text);
            renderer.DrawString(font, text, new Vector2(Screen.WIDTH, Screen.HEIGHT - size.Y * 2f), Color.White, 0, new Vector2(size.X, size.Y), Vector2.One);

            renderer.End();

            base.Draw(renderer);
        }

        public override SceneEnum NextScene()
        {
            return SceneEnum.GameScene;
        }
    }
}
