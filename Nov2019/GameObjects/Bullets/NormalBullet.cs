using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Nov2019.Devices;

namespace Nov2019.GameObjects.Bullets
{
    class NormalBullet : GameObject
    {
        Vector3 velocity;
        Vector3 direction;
        float moveSpeed;

        public NormalBullet(Vector3 position, Vector3 direction, float moveSpeed)
        {
            Position = position;
            this.direction = direction;
            this.moveSpeed = moveSpeed;
        }

        public override void Initialize()
        {
            velocity = direction * moveSpeed;
        }

        public override void Update()
        {
            Position += velocity * Time.Speed;
        }

        public override void Draw(Renderer renderer)
        {
            // 描画
            Matrix world =
                Matrix.CreateScale(1) *
                Matrix.CreateRotationX(MathHelper.ToRadians(0)) *
                Matrix.CreateRotationY(MathHelper.ToRadians(0)) *
                Matrix.CreateRotationZ(MathHelper.ToRadians(0)) *
                Matrix.CreateWorld(Position, Vector3.Forward, Vector3.Up);

            renderer.Draw3D("LowSphere", Color.MonoGameOrange, Camera, world);
        }

        public override void DrawUI(Renderer renderer)
        {

        }

        public override void HitAction(GameObject gameObject)
        {

        }
    }
}
