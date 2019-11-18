using Nov2019.Devices;
using Nov2019.GameObjects.Bullets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nov2019.GameObjects.BossAttackModules
{
    class AntiAir_AM : AttackModule
    {
        int count;
        int limit = 20;

        float shotTime;
        float shotLimit = 0.1f;

        public AntiAir_AM(BossEnemy BossEnemy) : base(BossEnemy)
        {
            count = 0;
            shotTime = -1f;
        }

        public override void Attack()
        {
            shotTime += Time.deltaTime;
            if (shotTime >= shotLimit)
            {
                count++;
                shotTime = 0;
                ObjectsManager.AddGameObject(new AntiAir_BossBullet(BossEnemy.Position), false);
                ObjectsManager.AddGameObject(new AntiAir_BossBullet(BossEnemy.Position + (BossEnemy as BossEnemy).AngleVec3 * 50), false);
                ObjectsManager.AddGameObject(new AntiAir_BossBullet(BossEnemy.Position + (BossEnemy as BossEnemy).AngleVec3 * 100), false);

                if (count >= limit)
                {
                    IsEndFlag = true;
                }
            }
        }
    }
}
