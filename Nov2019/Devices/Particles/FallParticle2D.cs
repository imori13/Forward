using System;using Microsoft.Xna.Framework;

namespace Nov2019.Devices.Particles
{
    class FallParticle2D : Particle2D
    {
        Random rand;

        public FallParticle2D(
            Vector2 position,
            Color color,
            Vector2 direction,
            Random rand)
            : base(
                  "Pixel",
                  rand.Next(1, 10) + (float)rand.NextDouble(),
                  position + direction * (rand.Next(80,100) + (float)rand.NextDouble()*Screen.ScreenSize),
                  direction,
                  rand.Next(0, 10) + (float)rand.NextDouble(), // speed
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
        }

        public override void Update()
        {
            base.Update();

            if (speed <= 0.01f)
            {
                IsDead = true;
            }

            scale = new Vector2(scale.X + speed * 0.01f, scale.Y + speed);
            scale = Vector2.Lerp(scale, Vector2.Zero, aliveRate);
        }

        public override void Draw(Renderer renderer,Camera camera)
        {
            renderer.Draw2D(
                   assetName,
                   position,
                   color,
                   rotation,
                   scale * Screen.ScreenSize);
        }
    }
}
