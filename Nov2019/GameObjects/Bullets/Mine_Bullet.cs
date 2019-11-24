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
    class Mine_Bullet : GameObject
    {
        Vector3 direction;
        float moveSpeed;
        float delay;

        float aliveTime;
        float aliveLimit;

        float particleTime;
        float particleLimit = 0.01f;

        Vector3 rotation;
        Vector3 rotation_speed;

        public Mine_Bullet(Vector3 position, Vector3 direction, float moveSpeed)
        {
            GameObjectTag = GameObjectTag.EnemyBullet;
            Position = position;
            this.direction = direction;
            this.moveSpeed = moveSpeed;
            delay = 0.1f;
        }

        public override void Initialize()
        {
            Position += direction * 20;

            aliveLimit = MyMath.RandF(4, 5);

            rotation = new Vector3(0, MyMath.RandF(360), 0);
            rotation_speed = new Vector3(10, 10, 10);
        }

        public override void Update()
        {
            Position += direction * moveSpeed * Time.deltaSpeed;

            moveSpeed -= delay * Time.deltaSpeed;
            moveSpeed = MathHelper.Clamp(moveSpeed, 0, float.MaxValue);

            particleTime += Time.deltaTime;
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

                ObjectsManager.AddGameObject(new DamageCollision(Position,50), false);

                for (int i = 0; i < 8; i++)
                {
                    ObjectsManager.AddParticle(new Spark_Particle3D(Position, MyMath.RandomCircleVec3(), GameDevice.Instance().Random));
                }

                for (int i = 0; i < 8; i++)
                {
                    ObjectsManager.AddParticle(new ExplosionParticle3D(Position, MyMath.RandomCircleVec3(), 5, GameDevice.Instance().Random));
                }
            }

            rotation += rotation_speed * Time.deltaSpeed;
        }

        public override void Draw(Renderer renderer)
        {
            // 描画
            Matrix world =
                Matrix.CreateScale(3) *
                Matrix.CreateRotationX(MathHelper.ToRadians(rotation.X)) *
                Matrix.CreateRotationY(MathHelper.ToRadians(rotation.Y)) *
                Matrix.CreateRotationZ(MathHelper.ToRadians(rotation.Z)) *
                Matrix.CreateWorld(Position, Vector3.Forward, Vector3.Up);

            renderer.Draw3D("Mine", "MineTexture", Camera, world);
        }

        public override void DrawUI(Renderer renderer)
        {

        }

        public override void HitAction(GameObject gameObject)
        {

        }
    }
}
