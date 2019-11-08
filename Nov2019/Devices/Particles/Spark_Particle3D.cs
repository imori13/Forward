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

        public Spark_Particle3D(
            Vector3 position,
            Vector3 direction,
            Random rand
            ) : base(
                "cube",
                Color.Lerp(Color.Yellow, Color.White, (float)rand.NextDouble()),
                MyMath.RandF(1, 2) * 0.25f,
                position,
                direction,
                MyMath.RandF(10, 100) * 0.001f,  // speed
                0.9f,   // friction
                Vector3.One * MyMath.RandF(1, 10) * 0.1f,  // scale
                Vector3.Zero,   // rotation
                Vector3.Zero,  // rotationspeed
                new Vector3(0.5f, 0.5f, 1.0f)  // origin
                )
        {
            speed *= 1 / scale.Length() * 25;
        }

        public override void Initialize()
        {
            base.Initialize();

            initscale = scale;

        }

        public override void Update()
        {
            base.Update();

            scale = Vector3.Lerp(initscale, Vector3.Zero, aliveRate);
            scale.Z = speed;
        }
        public override void Draw(Renderer renderer, Camera camera)
        {

            float latitude = (float)Math.Atan2(direction.X,direction.Y);
            float longitude = (float)Math.Atan2(direction.X, direction.Z);

            Matrix world =
                Matrix.CreateScale(scale) *
                Matrix.CreateRotationX(0) *
                Matrix.CreateRotationY(longitude) *
                Matrix.CreateRotationZ(0) *
                Matrix.CreateWorld(position + origin, Vector3.Forward, Vector3.Up);

            renderer.Draw3D(
            modelName,
            modelColor,
            camera,
            world);
        }
    }
}
