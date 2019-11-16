using Microsoft.Xna.Framework;
using Nov2019.Devices;
using Nov2019.GameObjects.MoveModules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nov2019.GameObjects.BossMoveModules
{
    class Rotate_MM : MoveModule
    {
        Vector3 destVelocity;   // 目標移動量
        float moveSpeed = 1;

        public Rotate_MM(BossEnemy BossEnemy) : base(BossEnemy)
        {
        }

        public override void Move()
        {
            destVelocity = BossEnemy.AngleVec3 * moveSpeed;

            BossEnemy.DestAngle += 0.5f* Time.Speed;

            BossEnemy.Velocity = Vector3.Lerp(BossEnemy.Velocity, destVelocity, 0.1f * Time.Speed);

            BossEnemy.Position += BossEnemy.Velocity * Time.Speed;
        }
    }
}
