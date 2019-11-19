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

        float deathTime;
        float deathLimit = 5;

        public Missile_AM(BossEnemy BossEnemy) : base(BossEnemy)
        {
            time = -1;
            deathTime = 0;
        }

        public override void Attack()
        {
            if (count >= countLimit)
            {
                deathTime += Time.deltaTime;

                if (deathTime >= deathLimit)
                {
                    IsEndFlag = true;
                }
            }
            else
            {
                time += Time.deltaTime;

                if (time >= limit)
                {
                    time = 0;
                    count++;

                    ObjectsManager.AddGameObject(new Missile_Bullet(BossEnemy.Position, MyMath.RandomCircleVec3()), false);
                }
            }
        }
    }
}
