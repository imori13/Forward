using Nov2019.Devices;
using Nov2019.GameObjects.BossAttackModules;
using Nov2019.GameObjects.BossMoveModules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nov2019.GameObjects.AttackModuleManages
{
    class Stage03AMManage : AttackModuleManage
    {
        public Stage03AMManage(BossEnemy bossEnemy) : base(bossEnemy)
        {

        }

        public override void Update()
        {
            if (AttackModule.IsEndFlag)
            {
                ChangeModule();

                return;
            }

            AttackModule.Attack();
            MoveModule.Move();
        }

        public override void ChangeModule()
        {
            AttackModule = new Missile_AM(bossEnemy);
            MoveModule = new Chase_MM(bossEnemy);
        }
    }
}
