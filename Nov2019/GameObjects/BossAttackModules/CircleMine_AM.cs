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
        static int count = 360 / 5;

        public CircleMine_AM(BossEnemy BossEnemy) : base(BossEnemy, "Mine_icon", 0.05f, count, 30)
        {

        }

        public override void Shot()
        {
            Random rand = GameDevice.Instance().Random;

            Vector2 vec2 = MyMath.DegToVec2(Count*5);
            float randY = MyMath.RandF(-1, 1) * 0.1f;
            Vector3 vec3 = new Vector3(vec2.Y, randY, vec2.X);
            ObjectsManager.AddGameObject(new Mine_Bullet(BossEnemy.Position, vec3, rand.Next(50, 100) * 0.1f), false);
        }
    }
}
