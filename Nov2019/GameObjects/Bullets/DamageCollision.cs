using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Nov2019.Devices;
using Nov2019.Devices.Collision;

namespace Nov2019.GameObjects.Bullets
{
    class DamageCollision : GameObject
    {
        float scale;
        bool flag;

        public DamageCollision(Vector3 position)
        {
            Position = position;
            scale = 50;
            Collider = new CircleCollider(this, scale);
            GameObjectTag = GameObjectTag.DamageCollision;
        }

        public override void Initialize()
        {
            flag = false;
        }

        public override void Update()
        {
            // １フレーム死亡をスキップ
            if (flag)
            {
                IsDead = true;
            }
            flag = true;
        }

        public override void Draw(Renderer renderer)
        {
            Matrix world =
                Matrix.CreateScale(scale) *
                Matrix.CreateRotationX(MathHelper.ToRadians(0)) *
                Matrix.CreateRotationY(MathHelper.ToRadians(0)) *
                Matrix.CreateRotationZ(MathHelper.ToRadians(0)) *
                Matrix.CreateWorld(Position, Vector3.Forward, Vector3.Up);

            renderer.Draw3D("LowSphere", Color.Red * 0.5f, Camera, world);
        }

        public override void DrawUI(Renderer renderer)
        {

        }

        public override void HitAction(GameObject gameObject)
        {

        }
    }
}
