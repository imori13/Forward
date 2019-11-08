using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Nov2019.Devices;
using Nov2019.Devices.Collision;

namespace Nov2019.GameObjects
{
    class Cube : GameObject
    {
        Vector3 rotation;
        Vector3 rotationSpeed;
        Color color;

        Random rand = GameDevice.Instance().Random;

        public Cube(Vector3 position)
        {
            Position = position;

            rotation = new Vector3(MyMath.RandF(360), MyMath.RandF(360), MyMath.RandF(360));

            float speed = 1;
            rotationSpeed = new Vector3(MyMath.RandF(-speed, speed), MyMath.RandF(-speed, speed), MyMath.RandF(-speed, speed));

            color = new Color(MyMath.RandF(200, 255), MyMath.RandF(200, 255), MyMath.RandF(200, 255));

            Collider = new CircleCollider(this, 1);

            GameObjectTag = GameObjectTag.Cube;
        }

        public override void Initialize()
        {

        }

        public override void Update()
        {
            rotation += rotationSpeed * Time.Speed;

            Position += Velocity;
            Velocity *= 0.95f;

            UpdateListPos();
        }

        public override void Draw(Renderer renderer)
        {
            Matrix world =
                Matrix.CreateScale(Vector3.One * 1) *
                Matrix.CreateRotationX(MathHelper.ToRadians(rotation.X)) *
                Matrix.CreateRotationY(MathHelper.ToRadians(rotation.Y)) *
                Matrix.CreateRotationZ(MathHelper.ToRadians(rotation.Z)) *
                Matrix.CreateWorld(Position, Vector3.Forward, Vector3.Up);

            renderer.Draw3D("cube", color, Camera, world);
        }

        public override void DrawUI(Renderer renderer)
        {

        }

        public override void HitAction(GameObject gameObject)
        {
            if (gameObject.GameObjectTag == GameObjectTag.Cube ||
                gameObject.GameObjectTag == GameObjectTag.Player)
            {
                BoundCircleCollision(gameObject);
            }
        }
    }
}
