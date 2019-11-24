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
        int maxScale;
        float scale;
        float aliveTime;
        static readonly float aliveLimit = 0.1f;
        Vector3 rotation;
        Vector3 rotation_speed;

        public DamageCollision(Vector3 position, int maxScale)
        {
            Position = position;
            this.maxScale = maxScale;
            scale = maxScale;
            Collider = new CircleCollider(this, scale);
            GameObjectTag = GameObjectTag.DamageCollision;

            rotation = new Vector3(MyMath.RandF(360), MyMath.RandF(360), MyMath.RandF(360));
            float speed = 10;
            rotation_speed = new Vector3(MyMath.RandF(-speed, speed), MyMath.RandF(-speed, speed), MyMath.RandF(-speed, speed));
        }

        public override void Initialize()
        {

        }

        public override void Update()
        {
            Collider = new CircleCollider(this, scale);
            UpdateListPos();

            rotation += rotation_speed * Time.deltaSpeed;

            aliveTime += Time.deltaTime;
            if (aliveTime >= aliveLimit)
            {
                IsDead = true;
            }

            scale = Easing2D.BackInOut(aliveTime, aliveLimit, maxScale, 0, 1);
        }

        public override void Draw(Renderer renderer)
        {
            Matrix world =
                Matrix.CreateScale(scale) *
                Matrix.CreateRotationX(MathHelper.ToRadians(rotation.X)) *
                Matrix.CreateRotationY(MathHelper.ToRadians(rotation.Y)) *
                Matrix.CreateRotationZ(MathHelper.ToRadians(rotation.Z)) *
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
