﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nov2019.GameObjects.BossMoveModules
{
    class None_MM : MoveModule
    {
        public None_MM(BossEnemy BossEnemy) : base(BossEnemy)
        {
        }

        public override void Move()
        {
            BossEnemy.DestVelocity = BossEnemy.AngleVec3 * 0;
        }
    }
}
