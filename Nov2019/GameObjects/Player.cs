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
        float movespeed = 2.5f;
        float rotateSpeed;

        public bool PlayerRightClickMode { get; private set; }

        float shotTime;

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
            Collider = new CircleCollider(this, 10);
            GameObjectTag = GameObjectTag.Player;
        }

        public override void Initialize()
        {

        }

        public override void Update()
        {
            Move();
            Rotation();
            UpdateListPos();

            // 左クリック押してるか
            // 押してたら攻撃する
            if (Input.IsLeftMouseHold())
            {
                float shotLimit = 0.05f;
                shotTime += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds;

                if (shotTime >= shotLimit)
                {
                    shotTime = 0;

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

            // 右クリック押してるか
            PlayerRightClickMode = Input.IsRightMouseHold();

            if (Input.GetKeyDown(Keys.G))
            {
                for (int i = 0; i < 100f; i++)
                {
                    ObjectsManager.AddParticle(new Spark_Particle3D(Position + AngleVec3 * 10f, MyMath.RandomCircleVec3(), GameDevice.Instance().Random));
                }
            }
        }

        public override void Draw(Renderer renderer)
        {
            // 描画
            Matrix world =
                Matrix.CreateScale(1) *
                Matrix.CreateRotationX(MathHelper.ToRadians(rotateX)) *
                Matrix.CreateRotationY(MathHelper.ToRadians(90 - Angle)) *
                Matrix.CreateRotationZ(MathHelper.ToRadians(0)) *
                Matrix.CreateWorld(Position, Vector3.Forward, Vector3.Up);

            renderer.Draw3D("boat", "boat_red", Camera, world);
        }

        public override void DrawUI(Renderer renderer)
        {
            SpriteFont font = Fonts.FontCica_32;
            string text;
            Vector2 size;

            text = "プレイヤー座標 X:" + Position.X.ToString("0000.00") + " Y:" + Position.Y.ToString("0000.00") + " Z:" + Position.Z.ToString("0000.00");
            size = font.MeasureString(text);
            renderer.DrawString(font, text, new Vector2(Screen.WIDTH / 2f, size.Y / 2f + size.Y * 0f), Color.White, 0, size / 2f, Vector2.One);

            text = "空間分割座標 : " + CurrentRootPos;
            size = font.MeasureString(text);
            renderer.DrawString(font, text, new Vector2(Screen.WIDTH / 2f, size.Y / 2f + size.Y * 1f), Color.White, 0, size / 2f, Vector2.One);
        }

        public override void HitAction(GameObject gameObject)
        {

        }

        // プレイヤーの移動処理
        private void Move()
        {
            // 移動・回転

            if (Input.GetKey(Keys.Space) ||
            Input.GetLeftTriggerButton(0) > 0.5f ||
            Input.GetRightTriggerButton(0) > 0.5f ||
            Input.IsPadButtonHold(Buttons.LeftShoulder, 0) ||
            Input.IsPadButtonHold(Buttons.RightShoulder, 0))
            {
                destVelocity = AngleVec3 * movespeed;

                Random rand = GameDevice.Instance().Random;
                for (int i = 0; i < 1; i++)
                {
                    ObjectsManager.AddParticle(new RocketFire_Particle3D(Position - AngleVec3 * 4f, -AngleVec3 + MyMath.RandomCircleVec3() * 0.15f, rand));
                }
            }

            Velocity = Vector3.Lerp(Velocity, destVelocity, 0.1f * Time.Speed);

            Position += Velocity * Time.Speed;

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

            rotateSpeed = (PlayerRightClickMode) ? (1.0f) : (3.0f);

            if (Input.GetKey(Keys.D) || Input.GetRightStickState(0).X > 0.5f || Input.GetLeftStickState(0).X > 0.5f || (PlayerRightClickMode && (Input.GetMousePosition().X - Screen.WIDTH / 2f) > 0.1f))
            {
                destAngle += rotateSpeed * Time.Speed;
                destRotateX = 25;
            }
            if (Input.GetKey(Keys.A) || Input.GetRightStickState(0).X < -0.5f || Input.GetLeftStickState(0).X < -0.5f || (PlayerRightClickMode && (Input.GetMousePosition().X - Screen.WIDTH / 2f) < -0.1f))
            {
                destAngle -= rotateSpeed * Time.Speed;
                destRotateX = -25;
            }

            // 船のロール回転
            rotateX = MathHelper.Lerp(rotateX, destRotateX, 0.05f * Time.Speed);
            // 船のヨー回転(左右回転)
            Angle = MathHelper.Lerp(Angle, destAngle, 0.1f * Time.Speed);
        }
    }
}
