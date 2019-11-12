using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Nov2019.Devices.Particles
{
    class Spark_Particle3D : Particle3D
    {
        Vector3 initscale;

        Vector3 initPos;

        public Spark_Particle3D(
            Vector3 position,
            Vector3 direction,
            Random rand
            ) : base(
                "cube",
                Color.Lerp(Color.Yellow, Color.White, (float)rand.NextDouble()),
                MyMath.RandF(1, 2) * 0.25f,
                position + direction * MyMath.RandF(0, 5),
                direction,
                MyMath.RandF(10, 50) * 0.001f,  // speed
                0.95f,   // friction
                Vector3.One * MyMath.RandF(5, 10) * 0.1f,  // scale
                Vector3.Zero,   // rotation
                Vector3.Zero,  // rotationspeed
                new Vector3(0.5f, 0.5f, 1.0f)  // origin
                )
        {
            speed *= 1 / scale.Length() * 25;
            initPos = position;
        }

        public override void Initialize()
        {
            base.Initialize();

            initscale = new Vector3(0.1f, 0.1f, 0.1f);
        }

        public override void Update()
        {
            base.Update();

            scale = Vector3.Lerp(initscale, new Vector3(0.1f + Vector3.Distance(initPos, position) * 0.1f, 0, 0), aliveRate);
        }
        public override void Draw(Renderer renderer, Camera camera)
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
            Matrix.CreateScale(scale) *
                Matrix.CreateFromAxisAngle(cross, (float)rad) *
                Matrix.CreateWorld(position, Vector3.Forward, Vector3.Up);

            renderer.Draw3D(
            modelName,
            modelColor,
            camera,
            world);
        }
    }
}
