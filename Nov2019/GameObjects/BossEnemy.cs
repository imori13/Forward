using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Nov2019.Devices;
using Nov2019.Devices.Collision;
using Nov2019.Devices.Particles;

namespace Nov2019.GameObjects
{
    class BossEnemy : GameObject
    {
        public float Angle { get; private set; }    // 向く角度
        float destAngle;
        Vector3 destVelocity;   // 目標移動量
        float moveSpeed = 0.5f;

        float fireTime;
        float fireLimit = 0.05f;

        float shotTime;
        float shotLimit = 0.1f;

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

        }

        public override void Update()
        {
            destVelocity = AngleVec3 * moveSpeed;

            Angle += 0.1f * Time.Speed;

            shotTime += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds * Time.Speed;
            if (shotTime >= shotLimit)
            {
                shotTime = 0;
                ObjectsManager.AddGameObject(new AntiAir_BossBullet(Position), false);
                ObjectsManager.AddGameObject(new AntiAir_BossBullet(Position + AngleVec3 * 50), false);
                ObjectsManager.AddGameObject(new AntiAir_BossBullet(Position + AngleVec3 * 100), false);
            }

            fireTime += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds * Time.Speed;

            if (fireTime >= fireLimit)
            {
                fireTime = 0;
                Random rand = GameDevice.Instance().Random;
                ObjectsManager.AddParticle(new BossRocketFire_Particle3D(Position - AngleVec3 * 50f, -AngleVec3 + MyMath.RandomCircleVec3() * 0.05f, rand));
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

        }

        public override void HitAction(GameObject gameObject)
        {

        }
    }
}
