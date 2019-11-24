using Microsoft.Xna.Framework;
using Nov2019.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nov2019.GameObjects.BossMoveModules
{
    class CircleRandom_MM : MoveModule
    {
        float time;
        static readonly float LIMIT = 2;
        float destAngle;

        public CircleRandom_MM(BossEnemy BossEnemy) : base(BossEnemy)
        {
            time = LIMIT;
        }

        public override void Move()
        {
            time += Time.deltaTime;

            if (time >= LIMIT)
            {
                time = MyMath.RandF(LIMIT);


                BossEnemy.DestAngle +=  MyMath.RandF(-45, 45);
            }

            BossEnemy.DestVelocity = BossEnemy.AngleVec3 * 1.5f;
        }
    }
}
