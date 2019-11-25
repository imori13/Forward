using Microsoft.Xna.Framework;
using Nov2019.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nov2019.GameObjects.BossAttackModules
{
    abstract class AttackModule
    {
        public string AssetName { get; private set; }
        public float ShotTime { get; private set; }
        public float ShotLimit { get; private set; }
        public int Count { get; private set; }
        public int CountLimit { get; private set; }
        public bool CoolTimeFlag { get; private set; }
        public float CoolTime { get; private set; }
        public float CoolTimeLimit { get; private set; }
        public BossEnemy BossEnemy { get; private set; }
        public ObjectsManager ObjectsManager { get; private set; }

        // 外部のUIのとこで使う情報。撃ったら一瞬赤色にする
        public bool ShotFlag { get; private set; }
        float time;
        float limit = 0.01f;

        public AttackModule(BossEnemy BossEnemy,string assetName, float shotLimit, int countLimit, float coolTimeLimit)
        {
            this.BossEnemy = BossEnemy;
            ObjectsManager = BossEnemy.ObjectsManager;
            ShotLimit = shotLimit;
            CountLimit = countLimit;
            CoolTimeLimit = coolTimeLimit;
            AssetName = assetName;
        }

        public void Initialize()
        {
            ShotTime = 0;
            Count = 0;
            CoolTime = 0;
            CoolTimeFlag = true;
            time = 0;
            ShotFlag = false;
        }

        public virtual void Attack()
        {
            if (ShotFlag)
            {
                time += Time.deltaTime;
                if (time >= limit)
                {
                    time = 0;
                    ShotFlag = false;
                }
            }


            // クールタイム
            if (CoolTimeFlag)
            {
                CoolTime += Time.deltaTime;
                if (CoolTime >= CoolTimeLimit)
                {
                    // プレイヤーが死んでいたら行わない
                    if (!ObjectsManager.Player.DeathFlag)
                    {
                        CoolTime = 0;
                        CoolTimeFlag = false;
                    }
                    else
                    {
                        CoolTime = MathHelper.Clamp(CoolTime, 0, CoolTimeLimit);
                    }
                }

                return;
            }

            ShotTime += Time.deltaTime;
            if (ShotTime >= ShotLimit)
            {
                Shot(); // 射撃

                ShotTime = 0;
                Count++;
                ShotFlag = true;
                time = 0;

                if (Count >= CountLimit)
                {
                    Initialize();
                }
            }
        }

        public abstract void Shot();
    }
}
