using System;using Microsoft.Xna.Framework;

namespace Nov2019.Devices.Particles
{
    class RadiationParticle2D : Particle2D
    {
        Random rand;

        float time;
        float limit=0.01f;

        public RadiationParticle2D(
            Vector2 position,
            Color color,
            Vector2 direction,
            Random rand)
            : base(
                  "Pixel",
                  rand.Next(1, 10) + (float)rand.NextDouble(),
                  position + direction * (rand.Next(80, 100) + (float)rand.NextDouble() * Screen.ScreenSize),
                  direction,
                  rand.Next(10,75) + (float)rand.NextDouble(), // speed
                  0.9f,    // friction
                  Color.Lerp(Color.White, color, (float)rand.NextDouble()),
                  0,
                  Vector2.One * 10,   // scale
                  MyMath.Vec2ToDeg(direction) - 90,    // rotation
                  0,
                  new Vector2(0.5f, 1))
        {
            this.rand = rand;
        }

        public override void Initialize()
        {
            base.Initialize();

            time = 0;
        }

        public override void Update()
        {
            base.Update();

            if (speed <= 0.01f)
            {
                IsDead = true;
            }

            time += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds * TimeSpeed.Time;
            if (time >= limit)
            {
                time = 0;
                scale = new Vector2(scale.X + speed * 0.001f, scale.Y + speed * 0.1f);
            }
            scale = Vector2.Lerp(scale, Vector2.Zero, aliveRate);
        }
    }
}
