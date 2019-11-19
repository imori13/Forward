using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nov2019.Devices;
using Nov2019.Devices.Collision;
using Nov2019.Devices.Particles;
using Nov2019.GameObjects.BossAttackModules;
using Nov2019.GameObjects.BossMoveModules;

namespace Nov2019.GameObjects
{
    class BossEnemy : GameObject
    {
        public enum BossStateEnum
        {
            Wait00, // 待機
            Stage01,
            Wait01, // やられ演出とか
            Stage02,
            Wait02,
        }

        public Vector3 DestVelocity { get; set; }

        public float BossHP { get; set; }

        public AttackModule AttackModule { get; set; }
        public MoveModule MoveModule { get; set; }

        // ボスの状態
        public BossStateEnum BossState { get; set; }

        public float DestAngle { get; set; }
        public float Angle { get; private set; }    // 向く角度

        float fireTime;
        float fireLimit = 0.05f;

        // 最初の待機状態の変数
        float wait00Time;
        float wait00Limit = 1.0f;

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

        public BossEnemy()
        {
            Position = new Vector3(0, 0, -500);
            Collider = new CircleCollider(this, 50);
            GameObjectTag = GameObjectTag.BossEnemy;
        }

        public override void Initialize()
        {
            BossState = BossStateEnum.Wait00;

            AttackModule = new None_AM(this);
            MoveModule = new Chase_MM(this);
        }

        public override void Update()
        {
            Velocity = Vector3.Lerp(Velocity, DestVelocity, 0.1f * Time.deltaSpeed);

            Position += Velocity * Time.deltaSpeed;

            BossStateManage();
            BossAMMMManage();

            AttackModule.Attack();
            MoveModule.Move();

            Angle = MathHelper.Lerp(Angle, DestAngle, 0.05f * Time.deltaSpeed);
        }

        public override void Draw(Renderer renderer)
        {
            // 描画
            Matrix world =
                Matrix.CreateScale(25) *
                Matrix.CreateRotationX(MathHelper.ToRadians(0)) *
                Matrix.CreateRotationY(MathHelper.ToRadians(90 - Angle)) *
                Matrix.CreateRotationZ(MathHelper.ToRadians(0)) *
                Matrix.CreateWorld(Position, Vector3.Forward, Vector3.Up);

            renderer.Draw3D("boat", "boat_blue", Camera, world);
        }

        public override void DrawUI(Renderer renderer)
        {
            SpriteFont font = Fonts.FontCica_32;
            string text;
            Vector2 size;

            float distance = Vector3.Distance(Position, ObjectsManager.Player.Position);
            text = "プレイヤーとボスの距離 : " + distance.ToString("000.00");
            size = font.MeasureString(text);
            renderer.DrawString(font, text, new Vector2(0, Screen.HEIGHT - size.Y * 0f), Color.White, 0, new Vector2(0, size.Y), Vector2.One);
        }

        public override void HitAction(GameObject gameObject)
        {

        }

        void BossStateManage()
        {
            switch (BossState)
            {
                case BossStateEnum.Wait00:
                    wait00Time += Time.deltaTime;
                    if (wait00Time >= wait00Limit)
                    {
                        BossState = BossStateEnum.Stage01;

                        AttackModule = new AntiAir_AM(this);
                        MoveModule = new Chase_MM(this);
                    }
                    break;
                case BossStateEnum.Stage01:

                    break;
                case BossStateEnum.Wait01:

                    break;
                case BossStateEnum.Stage02:

                    break;
                case BossStateEnum.Wait02:


                    break;
            }
        }

        void BossAMMMManage()
        {
            if (AttackModule.IsEndFlag)
            {
                Random rand = GameDevice.Instance().Random;
                switch (rand.Next(3))
                {
                    case 0: AttackModule = new AntiAir_AM(this); break;
                    case 1: AttackModule = new Housya_AM(this); break;
                    case 2: AttackModule = new Missile_AM(this); break;
                }

                //AttackModule = new AntiAir_AM(this);
            }

            if (MoveModule.IsEndFlag)
            {
                MoveModule = new None_MM(this);
            }
        }

        void MoveParticle()
        {
            // 移動パーティクル
            fireTime += Time.deltaTime;

            if (fireTime >= fireLimit)
            {
                fireTime = 0;
                Random rand = GameDevice.Instance().Random;
                ObjectsManager.AddParticle(new BossRocketFire_Particle3D(Position - AngleVec3 * 50f, -AngleVec3 + MyMath.RandomCircleVec3() * 0.05f, rand));
            }
        }

        void ClampPosition()
        {
            // 位置を補正
            Vector3 offset = new Vector3(10, 0, 10);

            float forcePower = 0.5f;
            if (Position.X < offset.X)
            {
                Velocity = new Vector3(forcePower, 0, Velocity.Z);
            }
            else if (Position.Z < offset.Z)
            {
                Velocity = new Vector3(Velocity.X, 0, forcePower);
            }
            else if (Position.X > ObjectsManager.MapLength - offset.X)
            {
                Velocity = new Vector3(-forcePower, 0, Velocity.Z);
            }
            else if (Position.Z > ObjectsManager.MapLength - offset.Z)
            {
                Velocity = new Vector3(Velocity.X, 0, -forcePower);
            }

            Position = Vector3.Clamp(Position, Vector3.Zero + offset, new Vector3(ObjectsManager.MapLength, 0, ObjectsManager.MapLength) - offset);
        }
    }
}
