using Nov2019.Devices;
using Nov2019.GameObjects.Bullets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nov2019.GameObjects.AttackModules
{
    class AntiAir_AM : AttackModule
    {
        float shotTime;
        float shotLimit = 0.1f;

        public AntiAir_AM(BossEnemy BossEnemy) : base(BossEnemy)
        {

        }

        public override void Attack()
        {

            shotTime += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds * Time.Speed;
            if (shotTime >= shotLimit)
            {
                shotTime = 0;
                ObjectsManager.AddGameObject(new AntiAir_BossBullet(BossEnemy.Position), false);
                ObjectsManager.AddGameObject(new AntiAir_BossBullet(BossEnemy.Position + (BossEnemy as BossEnemy).AngleVec3 * 50), false);
                ObjectsManager.AddGameObject(new AntiAir_BossBullet(BossEnemy.Position + (BossEnemy as BossEnemy).AngleVec3 * 100), false);
            }
        }
    }
}
