using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nov2019.Devices;
using Nov2019.Devices.Collision;
using Nov2019.Devices.Particles;

namespace Nov2019.GameObjects
{
    class Player : GameObject
    {
        public float Angle { get; private set; }    // プレイヤーの向く角度
        float destAngle;
        float rotateX;  // 船が傾く描写
        float destRotateX;
        Vector3 destVelocity;   // 目標移動量
        public static readonly float movespeed = 3f;
        float rotateSpeed;

        float shotTime;
        static readonly float shotLimit = 0.075f;

        float fireTime;
        static readonly float fireLimit = 0.01f;

        static readonly int MAX_HITPOINT = 5;
        public int HitPoint { get; private set; }
        public bool DeathFlag { get; private set; }

        public bool PlayerAimMode { get; private set; }

        public bool InvincibleFlag { get; private set; }    // 被弾時の無敵用フラグ
        static readonly int invincibleLimit = 4;
        float invincibleTime;

        public bool GAMESTART_FLAG { get; private set; }

        Color color;

        public float ShakeValue;

        // 角度をベクトルに変換するプロパティ
        public Vector3 AngleVec3
        {
            get
            {
                return new Vector3(
                    (float)Math.Cos(MathHelper.ToRadians(Angle - 90)),
                    0,
                    (float)Math.Sin(MathHelper.ToRadians(Angle - 90)));
            }
        }

        public Player()
        {
            Collider = new CircleCollider(this, 1f);
            GameObjectTag = GameObjectTag.Player;
        }

        public override void Initialize()
        {
            ShakeValue = 0;
            Position = new Vector3(0, 0, 1000);
            shotTime = 0;
            DeathFlag = false;
            GAMESTART_FLAG = false;
            fireTime = fireLimit;
            color = Color.White;
            HitPoint = MAX_HITPOINT;
            Angle = 0;
            destAngle = 0;
            rotateX = 0;
            Velocity = Vector3.Zero;
            destVelocity = Vector3.Zero;
            InvincibleFlag = false;
            invincibleTime = 0;
            CurrentRootPos = uint.MaxValue;
        }

        public override void Update()
        {
            if (!GAMESTART_FLAG)
            {
                Time.TitleStopMode = true;
                if (Input.GetKeyDown(Keys.Space) || Input.IsPadButtonDown(Buttons.A, 0) || Input.IsPadButtonDown(Buttons.B, 0))
                {
                    Time.TitleStopMode = false;
                    GAMESTART_FLAG = true;
                }
            }

            Move();
            Rotation();
            UpdateListPos();
            InvincibleManage();

            ShakeValue = MathHelper.Lerp(ShakeValue, 0, 0.1f * Time.deltaNormalSpeed);
            Input.SetVibration(0, ShakeValue);

            if (Input.GetKeyDown(Keys.G))
            {
                HitPoint = Math.Max(HitPoint - 1, 0);
                if (HitPoint <= 0)
                {
                    DeathFlag = true;
                    Time.PlayerDeathStopTime();
                    Camera.Shake(10, 5, 0.99f);
                    ShakeValue = 10000;
                }
                else
                {
                    InvincibleFlag = true;
                    Time.HitStop();
                    Camera.Shake(5, 1, 0.95f);
                    ShakeValue = 5;
                }
            }

            // 左クリック押してるか
            // 押してたら攻撃する
            if (!DeathFlag)
            {
                shotTime += Time.deltaTime;
                if (Input.IsLeftMouseHold() || Input.GetRightTriggerButton(0) >= 0.5f || Input.IsPadButtonHold(Buttons.RightShoulder, 0))
                {
                    if (shotTime >= shotLimit)
                    {
                        shotTime = 0;

                        ShakeValue = 0.5f;

                        Random rand = GameDevice.Instance().Random;
                        GameDevice.Instance().Sound.PlaySE("PlayerShot0" + rand.Next(1, 5).ToString());

                        Vector2 a = MyMath.DegToVec2(MyMath.Vec2ToDeg(new Vector2(AngleVec3.Z, AngleVec3.X)) + 90f);
                        Vector3 i = new Vector3(a.Y, 0, a.X);
                        ObjectsManager.AddGameObject(new PlayerBullet(Position + i * 1, AngleVec3), false);
                        ObjectsManager.AddGameObject(new PlayerBullet(Position - i * 1, AngleVec3), false);
                    }
                }
                else
                {
                    shotTime = 0;
                }
            }
        }

        public override void Draw(Renderer renderer)
        {
            if (DeathFlag) { return; }

            // 描画
            Matrix world =
                Matrix.CreateScale(0.75f) *
                Matrix.CreateRotationX(MathHelper.ToRadians(0)) *
                Matrix.CreateRotationZ(MathHelper.ToRadians(rotateX)) *
                Matrix.CreateRotationY(MathHelper.ToRadians(180 - Angle)) *
                Matrix.CreateWorld(Position, Vector3.Forward, Vector3.Up);

            renderer.Draw3D("Player", "PlayerTexture", color, Camera, world);

            if (MyDebug.DebugMode)
            {
                Matrix www =
               Matrix.CreateScale((Collider as CircleCollider).Radius) *
               Matrix.CreateRotationX(MathHelper.ToRadians(0)) *
               Matrix.CreateRotationY(MathHelper.ToRadians(0)) *
               Matrix.CreateRotationZ(MathHelper.ToRadians(0)) *
               Matrix.CreateWorld(Position, Vector3.Forward, Vector3.Up);

                renderer.Draw3D("LowSphere", Color.Red, Camera, www);
            }
        }

        public override void DrawUI(Renderer renderer)
        {
            SpriteFont font = Fonts.FontCica_32;
            string text;
            Vector2 size;

            if (!GAMESTART_FLAG)
            {
                renderer.Draw2D("SPACEPUSH_TO_START", Screen.Vec2 / 2f + Vector2.UnitY * 200 * Screen.ScreenSize, Color.White, 0, new Vector2(304, 72), Vector2.One * 1 * Screen.ScreenSize);
            }

            for (int i = 0; i < MAX_HITPOINT; i++)
            {
                string assetName = (HitPoint <= i) ? ("HPdeath_icon") : ("HPalive_icon");
                renderer.Draw2D(assetName, new Vector2(80 * Screen.ScreenSize + (i * 80 * Screen.ScreenSize), 60 * Screen.ScreenSize), Color.White, MathHelper.ToRadians(-45), Vector2.One * 40, Vector2.One * 0.8f * Screen.ScreenSize);
            }

            if (MyDebug.DebugMode)
            {
                text = "プレイヤー座標 { X:" + Position.X.ToString("0000.00") + " , Z:" + Position.Z.ToString("0000.00") + " } ";
                size = font.MeasureString(text);
                renderer.DrawString(font, text, new Vector2(0, size.Y / 2f * Screen.ScreenSize + size.Y * 0f * Screen.ScreenSize), Color.White, 0, new Vector2(0, size.Y / 2f), Vector2.One * Screen.ScreenSize);

                text = "空間分割座標 : " + CurrentRootPos;
                size = font.MeasureString(text);
                renderer.DrawString(font, text, new Vector2(0, size.Y / 2f * Screen.ScreenSize + size.Y * 1f * Screen.ScreenSize), Color.White, 0, new Vector2(0, size.Y / 2f), Vector2.One * Screen.ScreenSize);

                text = "プレイヤー被弾数 : " + HitPoint;
                size = font.MeasureString(text);
                renderer.DrawString(font, text, new Vector2(0, size.Y / 2f * Screen.ScreenSize + size.Y * 2f * Screen.ScreenSize), Color.White, 0, new Vector2(0, size.Y / 2f), Vector2.One * Screen.ScreenSize);

                text = "プレイヤー無敵フラグ : " + InvincibleFlag;
                size = font.MeasureString(text);
                renderer.DrawString(font, text, new Vector2(0, size.Y / 2f * Screen.ScreenSize + size.Y * 3f * Screen.ScreenSize), Color.White, 0, new Vector2(0, size.Y / 2f), Vector2.One * Screen.ScreenSize);
            }
        }

        public override void HitAction(GameObject gameObject)
        {
            bool returnFlag = (ObjectsManager.BossEnemy.isWait) || (InvincibleFlag) || (DeathFlag);
            if (returnFlag) { return; }

            if (gameObject.GameObjectTag == GameObjectTag.DamageCollision)
            {
                HitPoint = Math.Max(HitPoint - 1, 0);
                if (HitPoint <= 0)
                {
                    GameDevice.Instance().Sound.StopBGM();
                    DeathFlag = true;
                    Time.PlayerDeathStopTime();
                    Camera.Shake(5, 3, 0.96f);
                    ShakeValue = 50;
                }
                else
                {
                    GameDevice.Instance().Sound.PauseBGM();
                    InvincibleFlag = true;
                    Time.HitStop();
                    Camera.Shake(5, 1, 0.95f);
                    ShakeValue = 5;
                }
            }
        }

        // プレイヤーの移動処理
        private void Move()
        {
            // 移動
            if (!DeathFlag)
            {
                destVelocity = AngleVec3 * movespeed;
                fireTime += Time.deltaTime;

                if (fireTime >= fireLimit)
                {
                    fireTime = 0;
                    Random rand = GameDevice.Instance().Random;
                    ObjectsManager.AddParticle(new RocketFire_Particle3D(Position - AngleVec3 * 6, -AngleVec3 + MyMath.RandomCircleVec3() * 0.15f, rand));
                }

                Velocity = Vector3.Lerp(Velocity, destVelocity, 0.1f * Time.deltaSpeed);

                Position += Velocity * Time.deltaSpeed;
            }

            Vector3 offset = new Vector3(10, 0, 10);

            float forcePower = 0.5f;
            if (Position.X < offset.X)
            {
                Velocity = new Vector3(forcePower, 0, Velocity.Z);
                destVelocity = Vector3.Zero;
            }
            else if (Position.Z < offset.Z)
            {
                Velocity = new Vector3(Velocity.X, 0, forcePower);
                destVelocity = Vector3.Zero;
            }
            else if (Position.X > ObjectsManager.MapLength - offset.X)
            {
                Velocity = new Vector3(-forcePower, 0, Velocity.Z);
                destVelocity = Vector3.Zero;
            }
            else if (Position.Z > ObjectsManager.MapLength - offset.Z)
            {
                Velocity = new Vector3(Velocity.X, 0, -forcePower);
                destVelocity = Vector3.Zero;
            }

            Position = Vector3.Clamp(Position, Vector3.Zero + offset, new Vector3(ObjectsManager.MapLength, 0, ObjectsManager.MapLength) - offset);

            // プレイヤーの移動処理を最適化
            // ObjectManagerのPlayerMoveメソッドで移動を行う
        }

        // プレイヤーの回転処理
        private void Rotation()
        {
            destRotateX = 0;

            rotateSpeed = (PlayerAimMode) ? (1.0f) : (1.5f);


            // 右クリック押してるか
            PlayerAimMode = (Input.IsRightMouseHold() || Input.GetLeftTriggerButton(0) >= 0.5f) || Input.IsPadButtonHold(Buttons.LeftShoulder, 0) && !DeathFlag;

            if (!DeathFlag)
            {
                if (Input.GetKey(Keys.D) || Input.GetLeftStickState(0).X > 0.5f || (PlayerAimMode && (Input.GetMousePosition().X - Screen.WIDTH / 2) > 0.1f))
                {
                    destAngle += rotateSpeed * Time.deltaSpeed;
                    destRotateX = 25;
                }
                if (Input.GetKey(Keys.A) || Input.GetLeftStickState(0).X < -0.5f || (PlayerAimMode && (Input.GetMousePosition().X - Screen.WIDTH / 2) < -0.1f))
                {
                    destAngle -= rotateSpeed * Time.deltaSpeed;
                    destRotateX = -25;
                }
            }

            // 船のロール回転
            rotateX = MathHelper.Lerp(rotateX, destRotateX, 0.05f * Time.deltaSpeed);
            // 船のヨー回転(左右回転)
            Angle = MathHelper.Lerp(Angle, destAngle, 0.1f * Time.deltaSpeed);
        }

        void InvincibleManage()
        {
            if (InvincibleFlag)
            {
                invincibleTime += Time.deltaTime;


                color = (invincibleTime % 0.05f <= 0.025f) ? (color = Color.Black) : (color = Color.White);

                if (invincibleTime >= invincibleLimit)
                {
                    InvincibleFlag = false;
                    invincibleTime = 0;

                    color = Color.White;
                }
            }
        }
    }
}
