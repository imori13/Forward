using Microsoft.Xna.Framework;
using Nov2019.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nov2019.Devices.Collision
{
    // 軸に平行な矩形。回転はしない
    class BoxCollider : Collider
    {
        // 中心からのX,Y,Zの距離
        public Vector3 Size { get; private set; }

        public BoxCollider(GameObject gameobject, Vector3 size)
            : base(gameobject, ColliderEnum.Box)
        {
            Size = size;
        }

        public override bool IsCollision(Collider collider)
        {
            switch (collider.ColliderEnum)
            {
                case ColliderEnum.None: return false;
                case ColliderEnum.Box: return BoxCollision(collider as BoxCollider);
                case ColliderEnum.Circle: return CircleCollision(collider as CircleCollider);
            }

            return false;
        }

        public bool BoxCollision(BoxCollider collider)
        {
            if (gameobject.Position.X - Size.X / 2f < collider.gameobject.Position.X + collider.Size.X / 2f &&
                gameobject.Position.X + Size.X / 2f > collider.gameobject.Position.X - collider.Size.X / 2f &&
                gameobject.Position.Y - Size.Y / 2f < collider.gameobject.Position.Y + collider.Size.Y / 2f &&
                gameobject.Position.Y + Size.Y / 2f > collider.gameobject.Position.Y - collider.Size.Y / 2f &&
                gameobject.Position.Z - Size.Z / 2f < collider.gameobject.Position.Z + collider.Size.Z / 2f &&
                gameobject.Position.Z + Size.Z / 2f > collider.gameobject.Position.Z - collider.Size.Z / 2f)
            {
                return true;
            }

            return false;
        }

        // BoxとCircle
        public bool CircleCollision(CircleCollider collider)
        {
            Vector3 nearPoint = Vector3.Clamp(collider.gameobject.Position, gameobject.Position - Size, gameobject.Position + Size);

            if (Vector3.DistanceSquared(nearPoint, collider.gameobject.Position) < (collider.Radius * collider.Radius))
            {
                return true;
            }

            return false;
        }

        public override void Draw(Renderer renderer)
        {
            //Vector2 p1, p2, p3, p4;
            //p1 = gameobject.Position + new Vector2(-Size.X / 2f, -Size.Y / 2f);
            //p2 = gameobject.Position + new Vector2(Size.X / 2f, -Size.Y / 2f);
            //p3 = gameobject.Position + new Vector2(-Size.X / 2f, Size.Y / 2f);
            //p4 = gameobject.Position + new Vector2(Size.X / 2f, Size.Y / 2f);

            //float width = 1;
            //renderer.Draw2D("Pixel", gameobject.Position + new Vector2(0, -Size.Y / 2f), Color.LightGreen, 0, Vector2.One / 2f, new Vector2(p2.X - p1.X, width));
            //renderer.Draw2D("Pixel", gameobject.Position + new Vector2(Size.X / 2f, 0), Color.LightGreen, 0, Vector2.One / 2f, new Vector2(width, p3.Y - p2.Y));
            //renderer.Draw2D("Pixel", gameobject.Position + new Vector2(0, Size.Y / 2f), Color.LightGreen, 0, Vector2.One / 2f, new Vector2(p4.X - p3.X, width));
            //renderer.Draw2D("Pixel", gameobject.Position + new Vector2(-Size.X / 2f, 0), Color.LightGreen, 0, Vector2.One / 2f, new Vector2(width, p4.Y - p1.Y));
        }
    }
}
