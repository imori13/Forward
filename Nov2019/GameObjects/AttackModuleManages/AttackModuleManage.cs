using Nov2019.GameObjects.BossAttackModules;
using Nov2019.GameObjects.BossMoveModules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nov2019.GameObjects.AttackModuleManages
{
    abstract class AttackModuleManage
    {
        protected BossEnemy bossEnemy;
        protected ObjectsManager objectsManager;
        public AttackModule AttackModule { get; protected set; }
        public MoveModule MoveModule { get; protected set; }

        public AttackModuleManage(BossEnemy bossEnemy)
        {
            this.bossEnemy = bossEnemy;
            objectsManager = bossEnemy.ObjectsManager;
            AttackModule = new None_AM(bossEnemy);
            MoveModule = new None_MM(bossEnemy);
        }

        public abstract void Update();
        public abstract void ChangeModule();

        public void NoneModule()
        {
            AttackModule = new None_AM(bossEnemy);
            MoveModule = new None_MM(bossEnemy);
        }
    }
}
