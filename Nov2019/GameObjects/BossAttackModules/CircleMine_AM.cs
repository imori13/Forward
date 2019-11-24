using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Nov2019.Devices;
using Nov2019.GameObjects.Bullets;

namespace Nov2019.GameObjects.BossAttackModules
{
    class CircleMine_AM : AttackModule
    {
        public CircleMine_AM(BossEnemy BossEnemy) : base(BossEnemy, null, 1, 1, 5)
        {

        }

        public override void Shot()
        {
            Random rand = GameDevice.Instance().Random;

            for (int i = 0; i < 360; i += 5)
            {
                Vector2 vec2 = MyMath.DegToVec2(i);
                float randY = MyMath.RandF(-1, 1) * 0.1f;
                Vector3 vec3 = new Vector3(vec2.Y, randY, vec2.X);
                ObjectsManager.AddGameObject(new Mine_Bullet(BossEnemy.Position, vec3, rand.Next(50, 100) * 0.01f), false);
            }
        }
    }
}
