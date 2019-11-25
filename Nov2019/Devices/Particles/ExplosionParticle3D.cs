using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nov2019.Devices.Particles
{
    class ExplosionParticle3D : Particle3D
    {
        private Vector3 initScale;
        Color destColor;

        public ExplosionParticle3D(
            Vector3 position,
            Vector3 direction,
            float distance,
            float speed,
            Random random)
            : base(
                  "cube",
                  Color.Lerp(Color.Red, Color.Orange, random.Next(100) / 100f),
                  random.Next(0, 3) + (float)random.NextDouble(),
                  position + direction * distance,    // position
                  direction,
                  MyMath.RandF(0, speed),  //speed
                  0.9f,
                  Vector3.One * random.Next(300, 750) / 100f, //scale
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
            speed *= 1 / scale.Length() * 0.5f;

            destColor = Color.Lerp(Color.Black, Color.White, MyMath.RandF(1));
        }

        public override void Update()
        {
            base.Update();

            modelColor = Color.Lerp(modelColor, destColor, GetAliveRate());
            scale = initScale * (Math.Abs(GetAliveRate() - 1));
            if (scale.X <= 0.0001f || scale.Y <= 0.0001f || scale.Z <= 0.0001f)
            {
                IsDead = true;
            }
        }
    }
}
