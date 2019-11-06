using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Nov2019.Devices.Particles
{
    class ExplosionParticle2D : Particle2D
    {
        Vector2 initScale;

        public ExplosionParticle2D(
            Vector2 position,
            Vector2 direction,
            Color color,
            Random rand
            ) : base(
                "Pixel",
                rand.Next(0, 1) + (float)rand.NextDouble(),
                position,
                direction,
                rand.Next(10,30) + (float)rand.NextDouble(),  // speed
                0.9f,   // friction
                Color.Lerp(color, Color.White, (float)rand.NextDouble()),
                1,  // alpha
                Vector2.One * (rand.Next(20, 100) + (float)rand.NextDouble()),  // scale
                rand.Next(360) + (float)rand.NextDouble(),   // rotation
                rand.Next(-45, 45) + (float)rand.NextDouble(), // rotation_speed
                Vector2.One / 2f  // origin
                )
        {
            initScale = scale;
        }

        public override void Initialize()
        {
            speed *= 1 / scale.Length() * 25;

            base.Initialize();
        }

        public override void Update()
        {
            base.Update();

            scale = Vector2.Lerp(initScale, Vector2.Zero, GetAliveRate());
        }

        public override void Draw(Renderer renderer)
        {
            base.Draw(renderer);
        }
    }
}
