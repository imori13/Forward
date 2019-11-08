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
        float rotateSpeed = 1f;

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

            text = "プレイヤー座標 : " + Position;
            size = font.MeasureString(text);
            renderer.DrawString(font, text, new Vector2(Screen.WIDTH / 2f, size.Y / 2f + size.Y * 1f), Color.White, 0, size / 2f, Vector2.One);

            text = "空間分割座標 : " + CurrentRootPos;
            size = font.MeasureString(text);
            renderer.DrawString(font, text, new Vector2(Screen.WIDTH / 2f, size.Y / 2f + size.Y * 2f), Color.White, 0, size / 2f, Vector2.One);
        }

        public override void HitAction(GameObject gameObject)
        {

        }

        // プレイヤーの移動処理
        private void Move()
        {
            // 移動・回転
            destVelocity *= 0.95f;

            if (Input.GetKey(Keys.Space) ||
            Input.GetLeftTriggerButton(0) > 0.5f ||
            Input.GetRightTriggerButton(0) > 0.5f ||
            Input.IsPadButtonHold(Buttons.LeftShoulder, 0) ||
            Input.IsPadButtonHold(Buttons.RightShoulder, 0))
            {
                destVelocity = AngleVec3 * movespeed;
            }

            Velocity = Vector3.Lerp(Velocity, destVelocity, 0.01f * Time.Speed);

            Position += Velocity * Time.Speed;

            // プレイヤーの移動処理を最適化
            // ObjectManagerのPlayerMoveメソッドで移動を行う
        }

        // プレイヤーの回転処理
        private void Rotation()
        {
            destRotateX = 0;

            if (Input.GetKey(Keys.Right) || Input.GetRightStickState(0).X > 0.5f || Input.GetLeftStickState(0).X > 0.5f)
            {
                destAngle += rotateSpeed * Time.Speed;
                destRotateX = 25;
            }
            if (Input.GetKey(Keys.Left) || Input.GetRightStickState(0).X < -0.5f || Input.GetLeftStickState(0).X < -0.5f)
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
