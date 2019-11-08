using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Nov2019.Devices;

namespace Nov2019.GameObjects
{
    class Wall : GameObject
    {
        Vector3 scale;

        public Wall(Vector3 position, Vector3 scale)
        {
            Position = position;
            this.scale = scale;
        }

        public override void Initialize()
        {

        }

        public override void Update()
        {

        }
        public override void Draw(Renderer renderer)
        {

        }
        public override void DrawUI(Renderer renderer)
        {

        }

        public override void HitAction(GameObject gameObject)
        {

        }
    }
}
