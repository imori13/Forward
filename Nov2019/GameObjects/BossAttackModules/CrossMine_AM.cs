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
    class CrossMine_AM : AttackModule
    {
        int count = 0;
        int LimitCount = 20;
        float time;
        float limit = 0.05f;

        float deathTime;
        float deathLimit = 3;

        float initBossAngle;

        public CrossMine_AM(BossEnemy BossEnemy) : base(BossEnemy)
        {
            initBossAngle = BossEnemy.Angle;
        }

        public override void Attack()
        {
            if (count >= LimitCount)
            {
                deathTime += Time.deltaTime;
                if (deathTime >= deathLimit)
                {
                    IsEndFlag = true;
                }
            }
            else
            {
                time += Time.deltaTime;

                if (time >= limit)
                {
                    count++;

                    time = 0;

                    Random rand = GameDevice.Instance().Random;

                    for (int i = 0; i < 360; i += 90)
                    {
                        Vector2 vec2 = MyMath.DegToVec2(i+ initBossAngle);
                        float randY = MyMath.RandF(-1, 1) * 0.01f;
                        Vector3 vec3 = new Vector3(vec2.Y, randY, vec2.X);
                        ObjectsManager.AddGameObject(new Mine_Bullet(BossEnemy.Position, vec3, 5 * (count*0.5f) * 0.25f), false);
                    }
                }
            }
        }
    }
}
