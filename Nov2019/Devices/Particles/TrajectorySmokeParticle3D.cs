using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nov2019.Devices.Particles
{
    class TrajectorySmokeParticle3D : Particle3D
    {
        private Vector3 initScale;

        public TrajectorySmokeParticle3D(
            Vector3 position,
            Random random)
            : base(
                  "cube",
                  Color.Lerp(Color.White, Color.Black, random.Next(100) / 100f),
                   (float)random.NextDouble() * 0.25f,
                  position + Vector3.Up,    // position
                  MyMath.RandomCircleVec3(),
                  random.Next(0, 5) / 100f,  //speed
                  0.9f,
                  Vector3.One * random.Next(40, 100) / 100f, //scale
                  new Vector3(
                      random.Next(360) + (float)random.NextDouble(),
                      random.Next(360) + (float)random.NextDouble(),
                      random.Next(360) + (float)random.NextDouble()),
                  new Vector3(
                      random.Next(-360, 360) + (float)random.NextDouble(),
                      random.Next(-360, 360) + (float)random.NextDouble(),
                      random.Next(-360, 360) + (float)random.NextDouble()),
                  Vector3.Zero)
        {
            direction.Normalize();
            initScale = scale;
            speed *= 1 / scale.Length() / 0.5f;
        }

        public override void Update()
        {
            base.Update();

            position += direction * 0.1f * Time.Speed;

            scale = Vector3.Lerp(initScale, Vector3.Zero, GetAliveRate());
        }
    }
}
