using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Nov2019.Devices;
using Nov2019.Devices.Particles;

namespace Nov2019.GameObjects
{
    class AntiAir_BossBullet : GameObject
    {
        float speed;
        Player player;
        Vector3 explosionPosition;
        Vector3 direction;

        float particleTime;
        float particleLimit = 0.001f;

        public AntiAir_BossBullet(Vector3 position)
        {
            Position = position;
        }

        public override void Initialize()
        {
            speed = MyMath.RandF(5, 10);
            player = ObjectsManager.Player;

            explosionPosition = player.Position + MyMath.RandomCircleVec3() * MyMath.RandF(0, 200);

            direction = explosionPosition - Position;
            direction.Normalize();
        }

        public override void Update()
        {
            Velocity = direction * speed;

            Position += Velocity * Time.Speed;

            particleTime += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds * Time.Speed;

            if (particleTime >= particleLimit)
            {
                particleTime = 0;

                ObjectsManager.AddParticle(new TrajectorySmokeParticle3D(Position - Velocity * 1, GameDevice.Instance().Random));
            }

            if (Vector3.DistanceSquared(explosionPosition, Position) <= 100f)
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

            if (Vector3.DistanceSquared(ObjectsManager.Player.Position, Position) >= 1000 * 1000)
            {
                IsDead = true;
            }
        }

        public override void Draw(Renderer renderer)
        {
            // 回転軸は外積で計算できる
            var cross = Vector3.Cross(Vector3.Right, direction);

            // 内積の公式
            // dot = |V1||V2|cosθ
            // ここでV1とV2が正規化されているなら（＝長さ１）なら
            // dot = cosθ
            direction.Normalize();
            var dot = Vector3.Dot(Vector3.Right, direction);

            // cosθが分かったのでAcosで角度を計算
            var rad = Math.Acos(dot);

            Matrix world =
                Matrix.CreateWorld(Vector3.Left, Vector3.Forward, Vector3.Up) *
            Matrix.CreateScale(new Vector3(1f, 0.3f, 0.3f)) *
                Matrix.CreateFromAxisAngle(cross, (float)rad) *
                Matrix.CreateWorld(Position + new Vector3(0, 0.9f, 0), Vector3.Forward, Vector3.Up);

            renderer.Draw3D("cube", Color.Yellow, Camera, world);
        }

        public override void DrawUI(Renderer renderer)
        {

        }

        public override void HitAction(GameObject gameObject)
        {

        }
    }
}
