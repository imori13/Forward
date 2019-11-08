using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nov2019.Devices.Particles
{
    abstract class Particle
    {
        public bool IsDead;    // これがtrueならObjectManagerのListから削除される

        public abstract void Initialize();
        public abstract void Update();
        public abstract void Draw(Renderer renderer, Camera camera);
    }
}
