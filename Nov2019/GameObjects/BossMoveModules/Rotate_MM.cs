using Microsoft.Xna.Framework;
using Nov2019.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nov2019.GameObjects.BossMoveModules
{
    class Rotate_MM : MoveModule
    {
        float moveSpeed = 3;

        public Rotate_MM(BossEnemy BossEnemy) : base(BossEnemy)
        {
        }

        public override void Move()
        {
            BossEnemy.DestVelocity = BossEnemy.AngleVec3 * moveSpeed;

            BossEnemy.DestAngle += 0.5f * Time.deltaSpeed;
        }
    }
}
