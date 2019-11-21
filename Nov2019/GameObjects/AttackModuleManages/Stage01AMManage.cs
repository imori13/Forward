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
    class Stage01AMManage : AttackModuleManage
    {
        public Stage01AMManage(BossEnemy bossEnemy) : base(bossEnemy)
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
            Random rand = GameDevice.Instance().Random;
            switch (rand.Next(3))
            {
                case 0:
                    AttackModule = new AntiAir_AM(bossEnemy);
                    MoveModule = new Rotate_MM(bossEnemy);
                    break;
                case 1:
                    AttackModule = new CircleNormal_AM(bossEnemy);
                    MoveModule = new Rotate_MM(bossEnemy);
                    break;
                case 2:
                    AttackModule = new CrossMine_AM(bossEnemy);
                    MoveModule = new Rotate_MM(bossEnemy);
                    break;
            }

            AttackModule = new CrossMine_AM(bossEnemy);
            MoveModule = new Rotate_MM(bossEnemy);
        }
    }
}
