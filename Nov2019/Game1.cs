using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nov2019.Devices;
using Nov2019.Scenes;
using Nov2019.ScenesDevice;

namespace Nov2019
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        Renderer renderer;
        SceneManager sceneManager;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            Content.RootDirectory = "Content";

            // フルスクリーンのオンオフを設定
            graphics.IsFullScreen = true;
            // フルスクリーンモードから画面を切り替えると実行中のまま消えるアレをなくす処理
            graphics.HardwareModeSwitch = false;
            // DrawメソッドをモニタのVerticalRetraceと同期しない
            graphics.SynchronizeWithVerticalRetrace = false;
            // Updateメソッドをデフォルトのレート(1/60秒)で呼び出し
            IsFixedTimeStep = false;

            Screen.UpdateScreenSize(graphics, Window);


            sceneManager = new SceneManager();
        }

        protected override void Initialize()
        {
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GameDevice.Instance(GraphicsDevice, Content);
            Time.Initialize();
            MyDebug.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Fonts.LoadFonts(Content);

            renderer = GameDevice.Instance().Renderer;

            Texture2D Pixel = new Texture2D(GraphicsDevice, 1, 1);
            Color[] color = new Color[1];
            color[0] = Color.White;
            Pixel.SetData(color);
            renderer.LoadTexture("Pixel", Pixel);
            renderer.LoadTexture("slowMode", "Textures/");
            renderer.LoadTexture("Missile_icon", "Textures/");
            renderer.LoadTexture("AntiAir_icon", "Textures/");
            renderer.LoadTexture("Mine_icon", "Textures/");
            renderer.LoadTexture("HPalive_icon", "Textures/");
            renderer.LoadTexture("HPdeath_icon", "Textures/");  
            renderer.LoadTexture("Boss_icon", "Textures/");  
            renderer.LoadTexture("Cross_icon", "Textures/");  
            renderer.LoadTexture("STAGECLEAR_TEXT", "Textures/");
            renderer.LoadTexture("GAMECLEAR_TEXT", "Textures/");
            renderer.LoadTexture("THANKYOU_FOR_PLAYING_TEXT", "Textures/");  
            renderer.LoadTexture("SPACEHOLD_TO_RETURN_TEXT", "Textures/");  
            renderer.LoadTexture("GAMEOVER_TEXT", "Textures/");  
            renderer.LoadTexture("LbuttonUI", "Textures/");  
            renderer.LoadTexture("RbuttonUI", "Textures/");  
            renderer.LoadTexture("LStickUI", "Textures/");  
            renderer.LoadTexture("RStickUI", "Textures/");  
            renderer.LoadTexture("SPACEPUSH_TO_START", "Textures/");  

            renderer.Load3D("Player", "Models/");
            renderer.Load3D("boat", "Models/");
            renderer.Load3D("cube", "Models/");
            renderer.Load3D("LowSphere", "Models/");
            renderer.Load3D("Missile", "Models/");
            renderer.Load3D("Mine", "Models/");
            renderer.Load3D("Boss", "Models/");

            renderer.LoadTexture("PlayerTexture", "ModelTextures/");
            renderer.LoadTexture("MineTexture", "ModelTextures/");
            renderer.LoadTexture("boat_red", "ModelTextures/");
            renderer.LoadTexture("boat_blue", "ModelTextures/");
            renderer.LoadTexture("MissileTexture", "ModelTextures/");
            renderer.LoadTexture("BossTexture", "ModelTextures/");

            Sound sound = GameDevice.Instance().Sound;
            sound.LoadSE("PlayerShot01", "Sounds/");
            sound.LoadSE("PlayerShot02", "Sounds/");
            sound.LoadSE("PlayerShot03", "Sounds/");
            sound.LoadSE("PlayerShot04", "Sounds/");
            sound.LoadSE("Explosion01", "Sounds/");
            sound.LoadSE("Explosion02", "Sounds/");
            sound.LoadSE("Explosion03", "Sounds/");
            sound.LoadSE("Explosion04", "Sounds/");
            sound.LoadSE("BariaHit01", "Sounds/");
            sound.LoadSE("BariaHit02", "Sounds/");
            sound.LoadSE("BariaHit03", "Sounds/");
            sound.LoadSE("BariaBreak", "Sounds/");
            
            sound.LoadBGM("bgm_maoudamashii_8bit09", "BGMs/");
            sound.PlayBGM("bgm_maoudamashii_8bit09");
            sound.StopBGM();

            sceneManager.AddScene(SceneEnum.GameScene, new GameScene());

            sceneManager.ChangeScene(SceneEnum.GameScene);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            GameDevice.Instance().Update(gameTime);
            Time.Update();

            sceneManager.Update();
            Screen.Update(graphics, Window);
            // マウスを画面の中心に置く
            Input.SetMousePosition(Screen.WIDTH / 2, Screen.HEIGHT / 2);

            MyDebug.Update();
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(50, 40, 50));

            renderer.Begin2D();
            Time.Draw(renderer);
            renderer.End();

            sceneManager.Draw(renderer);

            base.Draw(gameTime);
        }
    }
}
