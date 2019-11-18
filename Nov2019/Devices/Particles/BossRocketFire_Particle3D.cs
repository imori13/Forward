using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nov2019.Devices.Particles
{
    class BossRocketFire_Particle3D : Particle3D
    {
        Vector3 initScale;

        public BossRocketFire_Particle3D(
            Vector3 position,
            Vector3 direction,
            Random rand
            ) : base(
                "cube",
                Color.Lerp(Color.Red, Color.Black, (float)rand.NextDouble()),
                MyMath.RandF(5, 10) * 1,
                position,
                direction,
                MyMath.RandF(10, 15) * 0.1f,  // speed
                0.9f,   // friction
                Vector3.One * MyMath.RandF(2,4),  // scale
                new Vector3(MyMath.RandF(360), MyMath.RandF(360), MyMath.RandF(360)),   // rotation
                new Vector3(MyMath.RandF(-45, 45), MyMath.RandF(-45, 45), MyMath.RandF(-45, 45)),  // rotationspeed
                new Vector3(0, 0, 0)  // origin
                )
        {
            speed *= 1 / scale.Length() * 25;
        }

        public override void Initialize()
        {
            initScale = scale;

            base.Initialize();
        }

        public override void Update()
        {
            scale = Vector3.Lerp(scale, Vector3.Zero, GetAliveRate() * Time.deltaSpeed);

            base.Update();
        }
        public override void Draw(Renderer renderer, Camera camera)
        {
            base.Draw(renderer, camera);
        }
    }
}
