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
        float initBossAngle;
        int deg;
        float distanceRate;

        public CrossMine_AM(BossEnemy BossEnemy,float shotLimit,int countLimit,float waitLimit,int deg,float distanceRate) : base(BossEnemy, "Mine_icon", shotLimit, countLimit, waitLimit)
        {
            initBossAngle = BossEnemy.Angle;
            this.deg = deg;
            this.distanceRate = distanceRate;
        }

        public override void Shot()
        {
            Random rand = GameDevice.Instance().Random;

            for (int i = 0; i < 360; i += deg)
            {
                Vector2 vec2 = MyMath.DegToVec2(i + initBossAngle);
                float randY = MyMath.RandF(-1, 1) * 0.01f;
                Vector3 vec3 = new Vector3(vec2.Y, randY, vec2.X);
                ObjectsManager.AddGameObject(new Mine_Bullet(BossEnemy.Position, vec3, 5 * (Count * distanceRate) * 0.25f), false);
            }
        }
    }
}
