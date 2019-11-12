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
            float distance = ObjectsManager.MapLength / 2f;

            for (int i = 0; i < 10000f; i++)
            {
                ObjectsManager.AddGameObject(new Cube(new Vector3(MyMath.RandF(-distance, distance), MyMath.RandF(-500, 500), MyMath.RandF(-distance, distance))), true);
            }

            ObjectsManager.AddGameObject(Player, true);
            ObjectsManager.AddGameObject(BossEnemy, true);

            base.Initialize();
        }

        public override void Update()
        {
            Camera.Update(Player);

            ObjectsManager.Update();

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

            text = "FPS値 : " + frameCounter.FPS.ToString("0.00").PadLeft(3, ' ');
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
