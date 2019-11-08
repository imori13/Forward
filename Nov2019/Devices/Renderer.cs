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
        // 描画用クラス
        private SpriteBatch spriteBatch;

        // テクスチャを格納
        private Dictionary<string, Texture2D> textures;
        // 3Dデータ
        private Dictionary<string, Model> models = new Dictionary<string, Model>();

        // カメラ描画情報
        private float fogS = 200;
        private float fogE = 700;
        private Color fogColor = Color.LightSkyBlue;
        private Vector3 AmbientLightColor = Vector3.One * 0.5f;
        private Vector3 EmissiveColor = Vector3.One * 0.25f;
        private Vector3 SpecularColor = Vector3.One * 1f;

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
            spriteBatch.Begin(
                  SpriteSortMode.Deferred,
                  BlendState.AlphaBlend,
                  SamplerState.LinearClamp,
                  DepthStencilState.Default,
                  RasterizerState.CullCounterClockwise,
                  null,
                  null);
        }

        // 描画終了
        public void End()
        {
            spriteBatch.End();
        }

        // 3Dデータの読み込み
        public void Load3D(string assetName, string filepath = "./")
        {
            Debug.Assert(ContentManager != null, "ModelRendererのContentManagerがnullです！");
            Debug.Assert(!models.ContainsKey(assetName), "すでにモデル" + assetName + "が読み込まれています");

            models.Add(assetName, ContentManager.Load<Model>(filepath + assetName));
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

        // テクスチャを指定してModelに貼る。描画処理
        public void Draw3D(string modelName, string modelTexture, Camera camera, Matrix world)
        {
            Debug.Assert(models.ContainsKey(modelName), "指定したAssetName[" + modelName + " ]または[ " + modelTexture + " ]がロードされていません。");

            foreach (ModelMesh mesh in models[modelName].Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.LightingEnabled = true;
                    effect.DirectionalLight0.Enabled = true;
                    effect.DirectionalLight0.Direction = new Vector3(0.1f, -0.5f, 1);
                    effect.DirectionalLight0.DiffuseColor = new Vector3(1, 1, 1);
                    effect.DirectionalLight0.SpecularColor = Color.White.ToVector3();
                    effect.AmbientLightColor = AmbientLightColor;
                    effect.EmissiveColor = EmissiveColor;
                    effect.SpecularColor = SpecularColor;

                    effect.World = world;
                    effect.View = camera.View;
                    effect.Projection = camera.Projection;

                    effect.FogEnabled = true;
                    effect.FogStart = fogS;
                    effect.FogEnd = fogE;
                    effect.FogColor = fogColor.ToVector3();

                    if (modelTexture != null && modelTexture != "")
                    {
                        Debug.Assert(textures.ContainsKey(modelTexture), "指定したModelTexture[ " + modelTexture + " ]が存在しません");

                        effect.DiffuseColor = Color.White.ToVector3();
                        effect.TextureEnabled = true;
                        effect.Texture = textures[modelTexture];
                    }
                }

                mesh.Draw();
            }
        }

        // 色を指定してモデルに色を塗る。描画処理
        public void Draw3D(string modelName, Color color, Camera camera, Matrix world)
        {
            Debug.Assert(models.ContainsKey(modelName), "指定したAssetName[" + modelName + " ]がロードされていません。");

            foreach (ModelMesh mesh in models[modelName].Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.LightingEnabled = true;
                    effect.EnableDefaultLighting();
                    effect.DirectionalLight0.Enabled = true;
                    effect.DirectionalLight0.Direction = new Vector3(0.1f, -0.5f, 1);
                    effect.DirectionalLight0.DiffuseColor = new Vector3(1, 1, 1);
                    effect.DirectionalLight0.SpecularColor = Color.White.ToVector3();
                    effect.AmbientLightColor = AmbientLightColor;
                    effect.EmissiveColor = EmissiveColor;
                    effect.SpecularColor = SpecularColor;

                    effect.World = world;
                    effect.View = camera.View;
                    effect.Projection = camera.Projection;

                    effect.FogEnabled = true;
                    effect.FogStart = fogS;
                    effect.FogEnd = fogE;
                    effect.FogColor = fogColor.ToVector3();

                    if (color != null)
                    {
                        effect.Alpha = color.A / 255f;
                        effect.DiffuseColor = color.ToVector3();
                    }
                }
                mesh.Draw();
            }
        }
    }
}
