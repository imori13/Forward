using Microsoft.Xna.Framework;
using Nov2019.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nov2019.Devices.Collision
{
    class CircleCollider : Collider
    {
        public float Radius { get; private set; }

        public CircleCollider(GameObject gameobject, float radius)
            : base(gameobject, ColliderEnum.Circle)
        {
            this.Radius = radius;
        }

        public override bool IsCollision(Collider collider)
        {
            switch (collider.ColliderEnum)
            {
                case ColliderEnum.None: return false;
                case ColliderEnum.Circle: return CircleCollision(collider as CircleCollider);
                case ColliderEnum.Box: return BoxCollision(collider as BoxCollider);
            }

            return false;
        }

        // 円と円の当たり判定
        bool CircleCollision(CircleCollider collider)
        {
            if (Vector3.DistanceSquared(gameobject.Position, collider.gameobject.Position)
                < (Radius + collider.Radius) * (Radius + collider.Radius))
            {
                return true;
            }

            return false;
        }

        // 円と四角形
        bool BoxCollision(BoxCollider collider)
        {
            Vector3 nearPoint =
                Vector3.Clamp(
                    gameobject.Position,
                    collider.gameobject.Position - collider.Size / 2f,
                    collider.gameobject.Position + collider.Size / 2f);

            if (Vector3.DistanceSquared(nearPoint, gameobject.Position) < (Radius * Radius))
            {
                return true;
            }

            return false;
        }

        public override void Draw(Renderer renderer)
        {
            //float width = 1;

            //List<Vector2> pos = new List<Vector2>();
            //for (int i = 0; i < 360; i += 10)
            //{
            //    pos.Add(gameobject.Position + MyMath.DegToVec2(i) * ((CircleCollider)gameobject.Collider).Radius);
            //}
            //for (int i = 0; i < pos.Count; i++)
            //{
            //    if (i == pos.Count - 1)
            //    {
            //        renderer.Draw2D("Pixel", pos[i], Color.LightGreen, MathHelper.ToRadians(MyMath.Vec2ToDeg(pos[0] - pos[i])), new Vector2(0, 0), new Vector2(Vector2.Distance(pos[0], pos[i]), width));
            //    }
            //    else
            //    {
            //        renderer.Draw2D("Pixel", pos[i], Color.LightGreen, MathHelper.ToRadians(MyMath.Vec2ToDeg(pos[i + 1] - pos[i])), new Vector2(0, 0), new Vector2(Vector2.Distance(pos[i + 1], pos[i]), width));
            //    }
            //}
        }
    }
}
