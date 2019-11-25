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
        public AntiAir_AM(BossEnemy BossEnemy, float shotLimit, int countLimit, float waitLimit) : base(BossEnemy, "AntiAir_icon", shotLimit, countLimit, waitLimit)
        {

        }

        public override void Attack()
        {
            //if (Vector3.DistanceSquared(BossEnemy.Position, ObjectsManager.Player.Position) <= 500 * 500)
            //{
            //    Initialize();

            //    return;
            //}

            base.Attack();
        }

        public override void Shot()
        {
            ObjectsManager.AddGameObject(new AntiAir_BossBullet(BossEnemy.Position), false);
            ObjectsManager.AddGameObject(new AntiAir_BossBullet(BossEnemy.Position + (BossEnemy as BossEnemy).AngleVec3 * 50), false);
            ObjectsManager.AddGameObject(new AntiAir_BossBullet(BossEnemy.Position + (BossEnemy as BossEnemy).AngleVec3 * 100), false);
        }
    }
}
