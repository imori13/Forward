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
    class AntiAir_BossBullet : GameObject
    {
        float speed;
        Player player;
        Vector3 explosionPosition;
        Vector3 direction;

        float particleTime;
        float particleLimit = 0.01f;

        float prevDistance;

        public AntiAir_BossBullet(Vector3 position)
        {
            GameObjectTag = GameObjectTag.EnemyBullet;
            Position = position;
            prevDistance = float.MaxValue;
        }

        public override void Initialize()
        {
            player = ObjectsManager.Player;

            float distance = Vector3.Distance(player.Position, Position);
            speed = MyMath.RandF(2, 4);

            // 時間 = 距離 / 速さ
            float time = distance / speed;

            // プレイヤーの 移動速度*時間 かける
            explosionPosition = (player.Position + player.Velocity * time) + MyMath.RandomCircleVec3() * MyMath.RandF(100);

            direction = explosionPosition - Position;
            direction.Normalize();
        }

        public override void Update()
        {
            Velocity = direction * speed;

            Position += Velocity * Time.deltaSpeed;

            particleTime += Time.deltaTime;

            if (particleTime >= particleLimit)
            {
                particleTime = 0;

                Random rand = GameDevice.Instance().Random;

                ObjectsManager.AddParticle(new TrajectorySmokeParticle3D(Position - Velocity * 1, (float)rand.NextDouble() * 0.25f, rand));
            }

            float currentDistance = Vector3.DistanceSquared(explosionPosition, Position);
            if (currentDistance >= prevDistance)
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

            prevDistance = currentDistance;
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

            renderer.Draw3D("cube", Color.Black, Camera, world);
        }

        public override void DrawUI(Renderer renderer)
        {

        }

        public override void HitAction(GameObject gameObject)
        {

        }
    }
}
