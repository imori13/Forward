using Microsoft.Xna.Framework;
using Nov2019.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nov2019.GameObjects.BossMoveModules
{
    class FirstPosition_MM : MoveModule
    {
        float time;
        float limit =8;
        float angle;

        public FirstPosition_MM(BossEnemy BossEnemy) : base(BossEnemy)
        {
            angle = BossEnemy.Angle - 90;
        }

        public override void Move()
        {
            //Vector3 distance = ObjectsManager.OffsetPosition - BossEnemy.Position;
            //Vector2 vec2 = new Vector2(distance.X, distance.Z);

            //float destAngle = MyMath.Vec2ToDeg(vec2);
            //BossEnemy.DestAngle = destAngle;

            BossEnemy.DestAngle = angle;

            BossEnemy.DestVelocity = BossEnemy.AngleVec3 * 3f;

            // 中心にある程度近づいたら終了
            time += Time.deltaTime;
            if (time >= limit)
            {
                IsEndFlag = true;
            }
        }
    }
}
