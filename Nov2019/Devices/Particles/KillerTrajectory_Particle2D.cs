using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Nov2019.Devices.Particles
{
    class KillerTrajectory_Particle2D : Particle2D
    {
        Vector2 initScale;

        public KillerTrajectory_Particle2D(
            Vector2 position,
            Vector2 direction,
            Random rand)
            : base(
                  "Pixel",
                  rand.Next(2, 4) + (float)rand.NextDouble(),
                  position,
                  MyMath.DegToVec2(MyMath.Vec2ToDeg(direction) + rand.Next(-10, 10) + (float)rand.NextDouble()),
                  rand.Next(2, 4) + (float)rand.NextDouble(),
                  0.98f,
                  Color.Lerp(Color.White, Color.Black, (float)rand.NextDouble()),
                  1,
                  Vector2.One * (rand.Next(5, 15) + (float)rand.NextDouble()),   // size
                  45, // rotation
                  rand.Next(-90, 90) + (float)rand.NextDouble(),    // rotationSpeed
                  Vector2.One * 0.5f)
        {
            initScale = scale;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update()
        {
            base.Update();

            scale = Vector2.Lerp(initScale, Vector2.Zero, GetAliveRate());
        }

        public override void Draw(Renderer renderer,Camera camera)
        {
            base.Draw(renderer,camera);
        }
    }
}
