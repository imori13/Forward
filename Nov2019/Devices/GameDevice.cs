using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace Nov2019.Devices
{
    sealed class GameDevice
    {
        // 唯一のインスタンス
        private static GameDevice instance;

        public GraphicsDevice Graphics { get; private set; }
        public ContentManager Content { get; private set; }
        public Renderer Renderer { get; private set; }
        public Sound Sound { get; private set; }
        public GameTime GameTime { get; private set; }
        public Random Random { get; private set; }

        // プライベートコンストラクタ
        private GameDevice(GraphicsDevice graphics, ContentManager content)
        {
            Graphics = graphics;
            Content = content;
            Renderer = new Renderer(graphics, content);
            Random = new Random();
            Sound = new Sound(content);
        }

        // インスタンス生成用
        public static GameDevice Instance(GraphicsDevice graphics, ContentManager content)
        {
            if (instance == null)
            {
                instance = new GameDevice(graphics, content);
            }

            return instance;
        }

        // インスタンス生成後のインスタンス受け取り
        public static GameDevice Instance()
        {
            Debug.Assert(instance != null, "GameDeviceインスタンスを生成前に引数なしのメソッドを呼び出さないでください！！");

            return instance;
        }

        public void Update(GameTime gameTime)
        {
            GameTime = gameTime;
            Input.Update();
        }
    }
}
