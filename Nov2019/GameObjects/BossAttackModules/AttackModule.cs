using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nov2019.GameObjects.AttackModules
{
    abstract class AttackModule
    {
        public BossEnemy BossEnemy { get; private set; }
        public ObjectsManager ObjectsManager { get; private set; }
        public bool IsEndFlag { get; protected set; }

        public AttackModule(BossEnemy BossEnemy)
        {
            this.BossEnemy = BossEnemy;
            ObjectsManager = BossEnemy.ObjectsManager;
            IsEndFlag = false;
        }

        public abstract void Attack();
    }
}
