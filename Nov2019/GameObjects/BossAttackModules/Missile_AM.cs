using Nov2019.Devices;
using Nov2019.GameObjects.Bullets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nov2019.GameObjects.BossAttackModules
{
    class Missile_AM : AttackModule
    {
        int count;
        float time;
        static readonly float limit = 0.025f;
        static readonly int countLimit = 10;

        public Missile_AM(BossEnemy BossEnemy) : base(BossEnemy)
        {
            time = -1;
        }

        public override void Attack()
        {
            time += Time.deltaTime;

            if (time >= limit)
            {
                time = 0;
                count++;

                ObjectsManager.AddGameObject(new Missile_Bullet(BossEnemy.Position, MyMath.RandomCircleVec3()),false);
            }

            if (count >= countLimit)
            {
                IsEndFlag = true;
            }
        }
    }
}
