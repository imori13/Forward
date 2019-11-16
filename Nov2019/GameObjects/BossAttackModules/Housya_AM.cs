using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Nov2019.Devices;
using Nov2019.GameObjects.AttackModules;
using Nov2019.GameObjects.Bullets;

namespace Nov2019.GameObjects.BossAttackModules
{
    class Housya_AM : AttackModule
    {
        int count = 0;
        int LimitCount = 3;
        float time;
        float limit = 0.5f;

        public Housya_AM(BossEnemy BossEnemy) : base(BossEnemy)
        {

        }

        public override void Attack()
        {
            time += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds * Time.Speed;

            if (time >= limit)
            {
                count++;

                time = 0;

                for (int i = 0; i < 360; i += 2)
                {

                    Vector2 vec2 = MyMath.DegToVec2(i);
                    float randY = MyMath.RandF(-1, 1) * 0.01f;
                    Vector3 vec3 = new Vector3(vec2.Y, randY, vec2.X);
                    ObjectsManager.AddGameObject(new NormalBullet(BossEnemy.Position, vec3, 1), false);
                }
            }

            if (count >= LimitCount)
            {
                IsEndFlag = true;
            }
        }
    }
}
