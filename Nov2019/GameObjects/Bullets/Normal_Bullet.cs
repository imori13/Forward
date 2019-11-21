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
    class Normal_Bullet : GameObject
    {
        Vector3 velocity;
        Vector3 direction;
        float moveSpeed;

        float aliveTime;
        float aliveLimit;

        float particleTime;
        float particleLimit = 0.01f;

        public Normal_Bullet(Vector3 position, Vector3 direction, float moveSpeed)
        {
            GameObjectTag = GameObjectTag.EnemyBullet;
            Position = position;
            this.direction = direction;
            this.moveSpeed = moveSpeed;
        }

        public override void Initialize()
        {
            velocity = direction * moveSpeed;
            Position += direction * 20;

            aliveLimit = MyMath.RandF(4,5);
        }

        public override void Update()
        {
            Position += velocity * Time.deltaSpeed;

            particleTime +=  Time.deltaTime;
            if (particleTime >= particleLimit)
            {
                particleTime = 0;

                Random rand = GameDevice.Instance().Random;

                ObjectsManager.AddParticle(new TrajectorySmokeParticle3D(Position - Velocity * 1, (float)rand.NextDouble() * 0.25f, rand));
            }

            aliveTime += Time.deltaTime;
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
                Matrix.CreateScale(3) *
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
