using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Nov2019.Devices;
using Nov2019.Devices.Particles;

namespace Nov2019.GameObjects.Bullets
{
    class Missile_Bullet : GameObject
    {
        Player player;

        float time;
        static readonly float chaseLimit = 1f;
        static readonly float deathLimit = 8;
        static readonly float MAXMOVESPEED = 7;
        static readonly float MINMOVESPEED = 2;

        float destMoveSpeed;
        float moveSpeed;

        float particleTime;
        float particleLimit = 0.025f;

        Vector3 destDirection;
        Vector3 direction;

        float rotation;
        float rotationSpeed;

        public Missile_Bullet(Vector3 position, Vector3 direction)
        {
            Position = position;
            this.direction = direction;
        }

        public override void Initialize()
        {
            player = ObjectsManager.Player;
            time = 0;
            moveSpeed = 1;
            rotation = MyMath.RandF(360);
            rotationSpeed = GameDevice.Instance().Random.Next(2) == 0 ? 10 : -10;
        }

        public override void Update()
        {
            time += Time.deltaTime;

            // まだ追わない処理
            if (time <= chaseLimit)
            {
                Position += direction * moveSpeed * Time.deltaSpeed;
            }
            else
            {
                destDirection = player.Position - Position;
                destDirection.Normalize();

                direction = Vector3.Lerp(direction, destDirection, 0.025f * Time.deltaSpeed);

                float distance = Math.Abs(MathHelper.ToDegrees(Vector3.Dot(destDirection, direction)));

                distance = MathHelper.Clamp(distance, 0, 20);
                float rate = distance / 20;
                destMoveSpeed = MathHelper.Lerp(MINMOVESPEED, MAXMOVESPEED, rate);

                moveSpeed = MathHelper.Lerp(moveSpeed, destMoveSpeed, 0.05f * Time.deltaSpeed);

                Position += direction * moveSpeed * Time.deltaSpeed;
            }

            // 死亡する処理
            if (time >= deathLimit)
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

            particleTime += Time.deltaTime;

            if (particleTime >= particleLimit)
            {
                particleTime = 0;
                Random rand = GameDevice.Instance().Random;
                ObjectsManager.AddParticle(new TrajectorySmokeParticle3D(Position - Velocity * 1, MyMath.RandF(0, 1), rand));
            }

            rotation += rotationSpeed * Time.deltaSpeed;
        }

        public override void Draw(Renderer renderer)
        {
            var cross = Vector3.Cross(Vector3.Right, direction);

            direction.Normalize();
            var dot = Vector3.Dot(Vector3.Right, direction);

            var rad = Math.Acos(dot);

            Matrix world =
                Matrix.CreateScale(3) *
                Matrix.CreateRotationY(MathHelper.ToRadians(rotation)) *
                Matrix.CreateRotationZ(MathHelper.ToRadians(-90)) *
                Matrix.CreateFromAxisAngle(cross, (float)rad) *
                Matrix.CreateWorld(Position, Vector3.Forward, Vector3.Up);

            renderer.Draw3D("Missile", "MissileTexture", Camera, world);
        }

        public override void DrawUI(Renderer renderer)
        {

        }

        public override void HitAction(GameObject gameObject)
        {

        }
    }
}
