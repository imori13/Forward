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

        public ExplosionParticle3D(
            Vector3 position,
            Vector3 direction,
            Random random)
            : base(
                  "cube",
                  Color.Lerp(Color.Black, Color.White, random.Next(100) / 100f),
                  random.Next(5,7) + (float)random.NextDouble(),
                  position,    // position
                  direction,
                  MyMath.RandF(1,40),  //speed
                  0.9f,
                  Vector3.One * random.Next(10, 500) / 100f, //scale
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
            speed *= 1 / scale.Length()*0.5f;
        }

        public override void Update()
        {
            base.Update();

            scale = initScale * (Math.Abs(GetAliveRate() - 1));
            if (scale.X <= 0.0001f || scale.Y <= 0.0001f || scale.Z <= 0.0001f)
            {
                IsDead = true;
            }
        }
    }
}
