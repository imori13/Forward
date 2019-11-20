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
    class PlayerBullet : GameObject
    {
        Vector3 direction;
        float speed;

        public PlayerBullet(Vector3 Position, Vector3 direction)
        {
            this.Position = Position;
            this.direction = direction;

            Collider = new CircleCollider(this, 3);
        }

        public override void Initialize()
        {
            speed = 20;

            GameObjectTag = GameObjectTag.PlayerBullet;

            Vector2 distance = MyMath.DegToVec2(MyMath.Vec2ToDeg(new Vector2(direction.Z, direction.X)) + MyMath.RandF(-1, 1));
            direction += new Vector3(distance.Y, 0, distance.X);

            if (direction.Length() == 0)
            {
                IsDead = true;
            }
            else
            {
                direction.Normalize();
            }
        }

        public override void Update()
        {
            Velocity = direction * speed;

            Position += Velocity * Time.deltaSpeed;

            if (Vector3.DistanceSquared(ObjectsManager.Player.Position, Position) >= 1000 * 1000)
            {
                IsDead = true;
            }
        }

        public override void Draw(Renderer renderer)
        {
            // 描画
            Matrix world =
                Matrix.CreateScale(new Vector3(0.1f, 0.1f, 1f)) *
                Matrix.CreateRotationX(MathHelper.ToRadians(0)) *
                Matrix.CreateRotationY(MathHelper.ToRadians(MyMath.Vec2ToDeg(new Vector2(direction.Z, direction.X)))) *
                Matrix.CreateRotationZ(MathHelper.ToRadians(0)) *
                Matrix.CreateWorld(Position, Vector3.Forward, Vector3.Up);

            renderer.Draw3D("cube", Color.Yellow, Camera, world);
        }

        public override void DrawUI(Renderer renderer)
        {

        }

        public override void HitAction(GameObject gameObject)
        {
            if (gameObject.GameObjectTag == GameObjectTag.BossEnemy)
            {
                IsDead = true;

                for (int i = 0; i < 25; i++)
                {
                    ObjectsManager.AddParticle(new Spark_Particle3D(Position, MyMath.RandomCircleVec3(), GameDevice.Instance().Random));
                }

                for (int i = 0; i < 10; i++)
                {
                    ObjectsManager.AddParticle(new ExplosionParticle3D(Position, MyMath.RandomCircleVec3(), GameDevice.Instance().Random));
                }
            }
        }
    }
}
