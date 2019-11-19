using Microsoft.Xna.Framework;
using Nov2019.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nov2019.GameObjects.BossMoveModules
{
    class Chase_MM : MoveModule
    {
        float moveSpeed = 2;
        Vector3 direction;

        public Chase_MM(BossEnemy BossEnemy) : base(BossEnemy)
        {

        }

        public override void Move()
        {
            Vector3 destDirection = ObjectsManager.Player.Position - BossEnemy.Position;
            destDirection.Normalize();
            direction = Vector3.Lerp(direction, destDirection, 0.01f * Time.deltaSpeed);
            Vector2 vec2 = new Vector2(direction.X, direction.Z);
            float deg = MyMath.Vec2ToDeg(vec2)+90f;

            float distance = (deg - BossEnemy.DestAngle) % 360;
            if (Math.Abs(distance) >= 180)
            {
                distance = (distance > 0) ? (distance - 360) : (distance + 360);
            }
            BossEnemy.DestAngle += distance;

            BossEnemy.DestVelocity = BossEnemy.AngleVec3 * moveSpeed * Time.deltaSpeed;
        }
    }
}
