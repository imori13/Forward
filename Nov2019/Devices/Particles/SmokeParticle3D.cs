using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nov2019.Devices.Particles
{
    class SmokeParticle3D : Particle3D
    {
        private Vector3 initScale;
        float upY;

        public SmokeParticle3D(
            Vector3 position,
            Random random)
            : base(
                  "cube",
                  Color.Lerp(Color.White, new Color(100, 100, 100), random.Next(100) / 100f),
                   (float)random.NextDouble(),
                  position + Vector3.Up,    // position
                  new Vector3(
                      random.Next(-100, 100) / 100f,
                      random.Next(-100, 100) / 100f,
                      random.Next(-100, 100) / 100f),
                  random.Next(50, 150) / 100f,  //speed
                  0.9f,
                  Vector3.One * random.Next(100, 300) / 100f, //scale
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
            upY = random.Next(1, 10) / 100f;
        }

        public override void Update()
        {
            base.Update();

            Position += Vector3.Up * upY * Time.deltaSpeed;

            scale = Vector3.Lerp(initScale, Vector3.Zero, GetAliveRate());
        }
    }
}
