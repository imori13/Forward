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
        public Missile_AM(BossEnemy BossEnemy, float shotLimit, int countLimit, float waitLimit) : base(BossEnemy, "Missile_icon", shotLimit, countLimit, waitLimit)
        {

        }

        public override void Shot()
        {
            ObjectsManager.AddGameObject(new Missile_Bullet(BossEnemy.Position, MyMath.RandomCircleVec3()), false);
        }
    }
}
