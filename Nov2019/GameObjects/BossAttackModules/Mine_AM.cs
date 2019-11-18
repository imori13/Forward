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
    class Mine_AM : AttackModule
    {
        int count = 0;
        int LimitCount = 20;
        float time;
        float limit = 0.2f;

        public Mine_AM(BossEnemy BossEnemy) : base(BossEnemy)
        {
        }

        public override void Attack()
        {
            time += Time.deltaTime;

            if (time >= limit)
            {
                count++;

                time = 0;

                for (int i = -180; i < 180; i += 10)
                {
                    if (i <= -90 && i >= 90) { continue; }

                    Vector2 vec2 = MyMath.DegToVec2(-BossEnemy.Angle + i);
                    float randY = MyMath.RandF(-1, 1) * 0.1f;
                    Vector3 vec3 = new Vector3(vec2.Y, randY, vec2.X);
                    ObjectsManager.AddGameObject(new NormalBullet(BossEnemy.Position, vec3, 1f), false);
                }
            }

            if (count >= LimitCount)
            {
                IsEndFlag = true;
            }
        }
    }
}
