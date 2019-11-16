using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Nov2019.Devices;
using Nov2019.Devices.Collision;
using Nov2019.Devices.Particles;

namespace Nov2019.GameObjects.Bullets
{
    class NormalBullet : GameObject
    {
        Vector3 velocity;
        Vector3 direction;
        float moveSpeed;

        float aliveTime;
        float aliveLimit;

        float particleTime;
        float particleLimit = 0.01f;

        public NormalBullet(Vector3 position, Vector3 direction, float moveSpeed)
        {
            Position = position;
            this.direction = direction;
            this.moveSpeed = moveSpeed;
            Collider = new CircleCollider(this, 1);
        }

        public override void Initialize()
        {
            velocity = direction * moveSpeed;
            Position += direction * 20;

            aliveLimit = MyMath.RandF(4,5);
        }

        public override void Update()
        {
            Position += velocity * Time.Speed;
            UpdateListPos();

            particleTime += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds * Time.Speed;
            if (particleTime >= particleLimit)
            {
                particleTime = 0;

                ObjectsManager.AddParticle(new TrajectorySmokeParticle3D(Position - Velocity * 1, GameDevice.Instance().Random));
            }

            aliveTime += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds * Time.Speed;
            if (aliveTime >= aliveLimit)
            {
                IsDead = true;

                for (int i = 0; i < 10; i++)
                {
                    ObjectsManager.AddParticle(new Spark_Particle3D(Position, MyMath.RandomCircleVec3(), GameDevice.Instance().Random));
                }

                for (int i = 0; i < 10; i++)
                {
                    ObjectsManager.AddParticle(new ExplosionParticle3D(Position, MyMath.RandomCircleVec3(), GameDevice.Instance().Random));
                }
            }
        }

        public override void Draw(Renderer renderer)
        {
            // 描画
            Matrix world =
                Matrix.CreateScale(1) *
                Matrix.CreateRotationX(MathHelper.ToRadians(0)) *
                Matrix.CreateRotationY(MathHelper.ToRadians(0)) *
                Matrix.CreateRotationZ(MathHelper.ToRadians(0)) *
                Matrix.CreateWorld(Position, Vector3.Forward, Vector3.Up);

            renderer.Draw3D("LowSphere", Color.MonoGameOrange, Camera, world);
        }

        public override void DrawUI(Renderer renderer)
        {

        }

        public override void HitAction(GameObject gameObject)
        {

        }
    }
}
