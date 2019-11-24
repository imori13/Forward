using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Nov2019.Devices
{
    public static class Screen
    {
        // 現在の解像度
        public static int WIDTH;
        public static int HEIGHT;

        public static readonly int MaxWidth = 1920;
        public static readonly int MaxHeight = 1080;

        static float width;
        static float height;

        static float DestScreenSize;
        public static float ScreenSize;

        public static Vector2 Vec2 { get { return new Vector2(WIDTH, HEIGHT); } }

        public static void UpdateScreenSize(GraphicsDeviceManager graphics, GameWindow Window)
        {
            // フルスクリーンの有無でサイズを変更
            WIDTH = (graphics.IsFullScreen) ? (1920) : (1280);
            HEIGHT = (graphics.IsFullScreen) ? (1080) : (720);

            // サイズを変更
            graphics.PreferredBackBufferWidth = WIDTH;
            graphics.PreferredBackBufferHeight = HEIGHT;
            graphics.ApplyChanges();

            DestScreenSize = (graphics.IsFullScreen) ? (1) : (0.9f);

            ScreenSize = DestScreenSize;
            width = MaxWidth * DestScreenSize;
            height = MaxHeight * DestScreenSize;

            WIDTH = (int)width;
            HEIGHT = (int)height;

            // サイズを変更
            graphics.PreferredBackBufferWidth = WIDTH;
            graphics.PreferredBackBufferHeight = HEIGHT;

            Window.Position =
                new Point(
                    (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2) - (graphics.PreferredBackBufferWidth / 2),
                    (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2) - (graphics.PreferredBackBufferHeight / 2) - 35);

            graphics.ApplyChanges();
        }

        public static void Update(GraphicsDeviceManager graphics, GameWindow Window)
        {
            if (graphics.IsFullScreen) { return; }

            if (Input.GetKey(Keys.OemPlus))
            {
                DestScreenSize += 0.025f * Time.deltaNormalSpeed;
            }

            if (Input.GetKey(Keys.OemMinus))
            {
                DestScreenSize -= 0.025f * Time.deltaNormalSpeed;
            }

            DestScreenSize = MathHelper.Clamp(DestScreenSize, 0.25f, 0.9f);

            ScreenSize = MathHelper.Lerp(ScreenSize, DestScreenSize, 0.05f * Time.deltaNormalSpeed);

            width = MathHelper.Lerp(width, MaxWidth * DestScreenSize, 0.05f * Time.deltaNormalSpeed);
            height = MathHelper.Lerp(height, MaxHeight * DestScreenSize, 0.05f * Time.deltaNormalSpeed);

            WIDTH = (int)Math.Ceiling(width);
            HEIGHT = (int)Math.Ceiling(height);

            if (graphics.PreferredBackBufferWidth == WIDTH || graphics.PreferredBackBufferHeight == HEIGHT)
            {
                return;
            }

            // サイズを変更
            graphics.PreferredBackBufferWidth = WIDTH;
            graphics.PreferredBackBufferHeight = HEIGHT;

            Window.Position =
                new Point(
                    (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2) - (graphics.PreferredBackBufferWidth / 2),
                    (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2) - (graphics.PreferredBackBufferHeight / 2) - 35);

            graphics.ApplyChanges();
        }
    }
}
