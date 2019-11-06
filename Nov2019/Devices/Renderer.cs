using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nov2019.Devices
{
    class Renderer
    {
        public GraphicsDevice GraphicsDevice { get; private set; }
        public ContentManager ContentManager { get; private set; }

        // テクスチャを格納
        private Dictionary<string, Texture2D> textures;
        // 描画用クラス
        private SpriteBatch spriteBatch;

        // コンストラクタ
        public Renderer(GraphicsDevice graphicsDevice, ContentManager contentManager)
        {
            textures = new Dictionary<string, Texture2D>();
            spriteBatch = new SpriteBatch(graphicsDevice);

            GraphicsDevice = graphicsDevice;
            ContentManager = contentManager;
        }

        // 描画開始
        public void Begin()
        {
            spriteBatch.Begin();
        }

        // 描画順をレイヤーごとにわけるBegin
        public void BeginCloud(Camera camera)
        {
            spriteBatch.Begin(
                     SpriteSortMode.FrontToBack,
                     BlendState.AlphaBlend,
                     SamplerState.LinearClamp,
                     DepthStencilState.None,
                     RasterizerState.CullCounterClockwise,
                     null,
                     camera.Matrix);
        }

        // 描画開始
        public void Begin(Camera camera)
        {
            spriteBatch.Begin(
                  SpriteSortMode.Deferred,
                  BlendState.AlphaBlend,
                  SamplerState.LinearClamp,
                  DepthStencilState.None,
                  RasterizerState.CullCounterClockwise,
                  null,
                  camera.Matrix);
        }

        // 描画終了
        public void End()
        {
            spriteBatch.End();
        }

        // テクスチャをロード
        public void LoadTexture(string name, string filePath = "./")
        {
            Debug.Assert(!textures.ContainsKey(name), "すでに同じアセット名[ " + name + " ]で登録されているものがあります");

            textures.Add(name, ContentManager.Load<Texture2D>(filePath + name));
        }

        // テクスチャをロード
        public void LoadTexture(string name, Texture2D texture)
        {
            Debug.Assert(!textures.ContainsKey(name), "すでに同じアセット名[ " + name + " ]で登録されているものがあります");

            textures.Add(name, texture);
        }

        // 通常描画
        public void Draw2D(string name, Vector2 position, Color color, SpriteEffects spriteEffects = SpriteEffects.None)
        {
            Debug.Assert(textures.ContainsKey(name), "アセット名[ " + name + " ]が見つかりません。ロードされてないかアセット名を間違えています");

            Draw2D(name, position, color, 0.0f, Vector2.One, spriteEffects);
        }

        // 引数拡張
        public void Draw2D(string name, Vector2 position, Color color, float rotation, Vector2 scale, SpriteEffects spriteEffects = SpriteEffects.None)
        {
            Debug.Assert(textures.ContainsKey(name), "アセット名[ " + name + " ]が見つかりません。ロードされてないかアセット名を間違えています");

            Draw2D(name, position, null, color, rotation, scale, spriteEffects);
        }

        public void Draw2D(string name, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, float layer = 1, SpriteEffects spriteEffects = SpriteEffects.None)
        {
            Debug.Assert(textures.ContainsKey(name), "アセット名[ " + name + " ]が見つかりません。ロードされてないかアセット名を間違えています");

            spriteBatch.Draw(textures[name], position, null, color, rotation, origin, scale, spriteEffects, layer);
        }

        public void Draw2D(string name, Vector2 position, Rectangle rectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects spriteEffects = SpriteEffects.None)
        {
            Debug.Assert(textures.ContainsKey(name), "アセット名[ " + name + " ]が見つかりません。ロードされてないかアセット名を間違えています");

            spriteBatch.Draw(textures[name], position, rectangle, color, rotation, origin, scale, spriteEffects, 0);
        }

        // 矩形切り抜き描画
        public void Draw2D(string name, Vector2 position, Rectangle? rectangle, Color color, float rotation, Vector2 scale, SpriteEffects spriteEffects = SpriteEffects.None)
        {
            Debug.Assert(textures.ContainsKey(name), "アセット名[ " + name + " ]が見つかりません。ロードされてないかアセット名を間違えています");

            var texture = textures[name];
            Vector2 origin = new Vector2(texture.Width / 2f, texture.Height / 2f);

            spriteBatch.Draw(textures[name], position, rectangle, color, MathHelper.ToRadians(rotation), origin, scale, spriteEffects, 0);
        }

        // 文字列描画
        public void DrawString(SpriteFont font, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale)
        {
            spriteBatch.DrawString(font, text, position, color, rotation, origin, scale, SpriteEffects.None, 0);
        }
    }
}
