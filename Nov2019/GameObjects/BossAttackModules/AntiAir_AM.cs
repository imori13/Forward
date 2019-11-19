using Microsoft.Xna.Framework;
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
        int limit =25;

        float shotTime;
        float shotLimit = 0.1f;

        float deathTime;
        float deathLimit = 3;

        public AntiAir_AM(BossEnemy BossEnemy) : base(BossEnemy)
        {
            count = 0;
            shotTime = -1f;
        }

        public override void Attack()
        {
            if (Vector3.DistanceSquared(ObjectsManager.Player.Position, BossEnemy.Position) <= 400 * 400)
            {
                IsEndFlag = true;
                return;
            }

            if (count >= limit)
            {
                deathTime += Time.deltaTime;
                if (deathTime >= deathLimit)
                {
                    IsEndFlag = true;
                }
            }
            else
            {
                shotTime += Time.deltaTime;
                if (shotTime >= shotLimit)
                {
                    count++;
                    shotTime = 0;

                    ObjectsManager.AddGameObject(new AntiAir_BossBullet(BossEnemy.Position), false);
                    ObjectsManager.AddGameObject(new AntiAir_BossBullet(BossEnemy.Position + (BossEnemy as BossEnemy).AngleVec3 * 50), false);
                    ObjectsManager.AddGameObject(new AntiAir_BossBullet(BossEnemy.Position + (BossEnemy as BossEnemy).AngleVec3 * 100), false);
                }
            }
        }
    }
}
