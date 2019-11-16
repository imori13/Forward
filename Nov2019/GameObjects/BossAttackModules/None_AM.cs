using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nov2019.GameObjects.AttackModules
{
    class None_AM : AttackModule
    {
        public None_AM(BossEnemy BossEnemy) : base(BossEnemy)
        {
        }

        public override void Attack()
        {

        }
    }
}
