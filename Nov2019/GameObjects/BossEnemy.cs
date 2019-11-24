using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nov2019.Devices;
using Nov2019.Devices.Collision;
using Nov2019.Devices.Particles;
using Nov2019.GameObjects.BossAttackModules;
using Nov2019.GameObjects.BossMoveModules;
using Nov2019.GameObjects.Bullets;

namespace Nov2019.GameObjects
{
    class BossEnemy : GameObject
    {
        public enum BossStateEnum
        {
            Stage01,
            Stage02,
            Stage03,
        }

        public Vector3 DestVelocity { get; set; }

        // ボスの状態
        public BossStateEnum BossState { get; set; }

        // ボスのモジュール
        public List<AttackModule> AttackModules { get; private set; } = new List<AttackModule>();
        public MoveModule MoveModule { get; private set; }

        public float DestAngle
        { get; set; }
        public float Angle { get; private set; }    // 向く角度
        float fireTime;
        float fireLimit = 0.05f;

        // 最初の待機状態の変数
        float waitTime;
        static readonly float waitLimit = 2.5f;
        public bool isWait { get; private set; }
        bool isHeal;    // 回復状態か

        Color color;

        float collscale;
        float offsetScale;
        float offsetDestScale;
        static readonly int MIN_COLLSCALE = 30;
        static readonly int MAX_COLLSCALE = 90;
        float bariaAlpha;   // バリアの透明地
        float bariaRotateY;
        float bariaDestRotateY;

        float healRebootTime;
        static readonly float healRebootLimit = 3;

        public float BossHP { get; set; }
        public int DamageSumCount { get; private set; }
        public int DamageCount { get; private set; }    // HPが0になった回数
        static readonly int damageCountLimit = 3;

        public bool InvincibleFlag { get; private set; }
        float invincibleTime;
        static readonly float invincibleLimit = 8.0f;

        // 角度をベクトルに変換するプロパティ
        public Vector3 AngleVec3
        {
            get
            {
                return new Vector3(
                    (float)Math.Cos(MathHelper.ToRadians(Angle - 90)),
                    0,
                    (float)Math.Sin(MathHelper.ToRadians(Angle - 90)));
            }
        }

        public BossEnemy()
        {
            Position = new Vector3(0, 0, -500);
            Collider = new CircleCollider(this, 50);
            GameObjectTag = GameObjectTag.BossEnemy;
        }

        public override void Initialize()
        {
            BossState = BossStateEnum.Stage01;
            AttackModules.Clear();
            MoveModule = new None_MM(this);

            isWait = true;
            BossHP = 100;
            collscale = 50;
            color = Color.White;
            bariaDestRotateY = 0;
        }

        public override void Update()
        {
            Velocity = Vector3.Lerp(Velocity, DestVelocity, 0.1f * Time.deltaSpeed);

            Position += Velocity * Time.deltaSpeed;

            BossStateManage();

            AttackModules.ForEach(a => a.Attack());
            MoveModule.Move();

            BossHPManage();

            MoveParticle();

            InvincibleManage();

            float rate = BossHP / 100f;
            collscale = MathHelper.Lerp(collscale, Easing2D.ExpOut(BossHP, 100, MAX_COLLSCALE, MIN_COLLSCALE), 0.25f * Time.deltaSpeed);
            Collider = new CircleCollider(this, collscale);

            bariaAlpha = (isWait) ? (0) : (MathHelper.Lerp(bariaAlpha, 0.5f, 0.1f * Time.deltaSpeed));

            color = Color.Lerp(color, (isWait) ? (Color.Gray) : (Color.White), 0.1f * Time.deltaSpeed);

            bariaRotateY = MathHelper.Lerp(bariaRotateY, bariaDestRotateY, 0.1f * Time.deltaSpeed);
            offsetDestScale = MathHelper.Lerp(offsetDestScale, 0, 0.1f * Time.deltaSpeed);
            offsetScale = MathHelper.Lerp(offsetScale, offsetDestScale, 0.5f * Time.deltaSpeed);

            Angle = MathHelper.Lerp(Angle, DestAngle, 0.05f * Time.deltaSpeed);

            ClampPosition();
        }

        public override void Draw(Renderer renderer)
        {
            // 描画
            Matrix world;
            world =
                Matrix.CreateScale(10) *
                Matrix.CreateRotationX(MathHelper.ToRadians(0)) *
                Matrix.CreateRotationY(MathHelper.ToRadians(90 - Angle)) *
                Matrix.CreateRotationZ(MathHelper.ToRadians(0)) *
                Matrix.CreateWorld(Position, Vector3.Forward, Vector3.Up);

            renderer.Draw3D("boat", "boat_blue", color, Camera, world, false);

            if (isWait) { return; }

            world =
                Matrix.CreateScale(collscale + offsetScale) *
                Matrix.CreateRotationX(MathHelper.ToRadians(0)) *
                Matrix.CreateRotationY(MathHelper.ToRadians(90 - Angle + bariaRotateY)) *
                Matrix.CreateRotationZ(MathHelper.ToRadians(0)) *
                Matrix.CreateWorld(Position, Vector3.Forward, Vector3.Up);

            renderer.Draw3D("LowSphere", Color.Green * bariaAlpha, Camera, world);
        }

        public override void DrawUI(Renderer renderer)
        {
            SpriteFont font = Fonts.FontCica_32;
            string text;
            Vector2 size;

            // ボスのステージのUI
            float stageCount = 4;
            for (int i = 0; i < stageCount; i++)
            {
                Vector2 pos = new Vector2(Screen.WIDTH / 2f + (((i + 0.5f) - stageCount / 2f) * 200 * Screen.ScreenSize), 60 * Screen.ScreenSize);
                renderer.Draw2D("Pixel", pos, Color.White, MathHelper.ToRadians(22.5f), Vector2.One * 0.5f, Vector2.One * 160 * 0.4f * Screen.ScreenSize);

                Color color = (i < (int)BossState + 1) ? (new Color(215, 73, 73)) : (Color.Gray);

                renderer.Draw2D("Pixel", pos, color, MathHelper.ToRadians(22.5f), Vector2.One * 0.5f, Vector2.One * 130 * 0.4f * Screen.ScreenSize);

                if (i != 0)
                {
                    renderer.Draw2D("Boss_icon", pos, Color.White, 0, Vector2.One * 64, Vector2.One * Screen.ScreenSize * 0.4f);

                    if (i <= (int)BossState)
                    {
                        renderer.Draw2D("Cross_icon", pos, Color.White, MathHelper.ToRadians(30), Vector2.One * 64, Vector2.One * 0.5f * Screen.ScreenSize);
                    }
                }
            }

            // ボスのステージ内のステージのUI
            float hoge = 13;
            int count = 0;
            for (int i = 0; i < hoge; i++)
            {
                if (i == 0 || i % 4 == 0) { continue; }
                count++;
                Color color = (count <= DamageSumCount) ? (new Color(215, 73, 73)) : (Color.Gray);

                Vector2 pos = new Vector2(Screen.WIDTH / 2f + (((i + 0.5f) - hoge / 2f) * 50 * Screen.ScreenSize), 60 * Screen.ScreenSize);
                renderer.Draw2D("Pixel", pos, Color.White, MathHelper.ToRadians(22.5f), Vector2.One * 0.5f, Vector2.One * 20 * Screen.ScreenSize);
                renderer.Draw2D("Pixel", pos, color, MathHelper.ToRadians(22.5f), Vector2.One * 0.5f, Vector2.One * 15 * Screen.ScreenSize);
            }

            // ボスのステージ内のステージのHPのUI
            Vector2 offset = Vector2.One * 3 * Screen.ScreenSize;
            renderer.Draw2D("Pixel", new Vector2(Screen.WIDTH / 2f, 180 * Screen.ScreenSize), Color.Black, MathHelper.ToRadians(0), Vector2.One * 0.5f, new Vector2(500, 3) * Screen.ScreenSize + offset);
            if (!isWait && !InvincibleFlag)
            {
                renderer.Draw2D("Pixel", new Vector2(Screen.WIDTH / 2f, 180 * Screen.ScreenSize), new Color(200, 200, 200), MathHelper.ToRadians(0), Vector2.One * 0.5f, new Vector2(500 * (BossHP / 100), 3) * Screen.ScreenSize);
            }

            // 攻撃モジュールのUI
            for (int i = 0; i < AttackModules.Count; i++)
            {
                Vector2 pos = new Vector2(Screen.WIDTH / 2f + (((i + 0.5f) - AttackModules.Count / 2f) * 100 * Screen.ScreenSize), 150 * Screen.ScreenSize);    // アイコンの中心座標
                Color color = (AttackModules[i].ShotFlag) ? (Color.Yellow) : (Color.White); // 発射したら黄色になる
                float siiize = (AttackModules[i].ShotFlag) ? (75) : (70);   // 発射したらちょっと拡大する
                renderer.Draw2D("Pixel", pos, color, MathHelper.ToRadians(22.5f), Vector2.One * 0.5f, Vector2.One * siiize * Screen.ScreenSize);  // アイコンの背景後　アウトラインみたいなやつ
                renderer.Draw2D("Pixel", pos, Color.Gray, MathHelper.ToRadians(22.5f), Vector2.One * 0.5f, Vector2.One * 60 * Screen.ScreenSize); // アイコンの背景前

                string assetName = AttackModules[i].AssetName;
                if (assetName != null)
                    renderer.Draw2D(assetName, pos, Color.White, MathHelper.ToRadians(45), Vector2.One * 40, Vector2.One * Screen.ScreenSize);  // アイコン描画

                Vector2 scale = new Vector2(75, 10);
                renderer.Draw2D("Pixel", pos + Vector2.UnitY * 40 * Screen.ScreenSize, Color.Black, 0, Vector2.One * 0.5f, scale * Screen.ScreenSize + Vector2.One * 5 * Screen.ScreenSize);
                // クールタイム
                if (AttackModules[i].CoolTimeFlag)
                {
                    float coolTimeWidth = AttackModules[i].CoolTime / AttackModules[i].CoolTimeLimit;
                    renderer.Draw2D("Pixel", pos + Vector2.UnitY * 40 * Screen.ScreenSize - Vector2.UnitX * scale.X / 2f * Screen.ScreenSize, new Color(100, 100, 100), 0, new Vector2(0, 0.5f), new Vector2(scale.X * coolTimeWidth, scale.Y) * Screen.ScreenSize);
                }
                // 攻撃時
                else
                {
                    float countWitdh = AttackModules[i].Count / (float)AttackModules[i].CountLimit;
                    float shotWidht = AttackModules[i].ShotTime / AttackModules[i].ShotLimit;
                    renderer.Draw2D("Pixel", pos + (Vector2.UnitY * 40 * Screen.ScreenSize) - (Vector2.UnitY * scale.Y / 3f * Screen.ScreenSize) - Vector2.UnitX * scale.X / 2f * Screen.ScreenSize, new Color(200, 150, 150), 0, new Vector2(0, 0.5f), new Vector2(scale.X * shotWidht, scale.Y / 2f) * Screen.ScreenSize);
                    renderer.Draw2D("Pixel", pos + (Vector2.UnitY * 40 * Screen.ScreenSize) + (Vector2.UnitY * scale.Y / 3f * Screen.ScreenSize) - Vector2.UnitX * scale.X / 2f * Screen.ScreenSize, new Color(200, 50, 50), 0, new Vector2(0, 0.5f), new Vector2(scale.X * countWitdh, scale.Y / 2f) * Screen.ScreenSize);
                }
            }

            if (MyDebug.DebugMode)
            {
                float distance = Vector3.Distance(Position, ObjectsManager.Player.Position);
                text = "プレイヤーとボスの距離 : " + distance.ToString("000.00");
                size = font.MeasureString(text);
                renderer.DrawString(font, text, new Vector2(0, Screen.HEIGHT - size.Y * 0f * Screen.ScreenSize), Color.White, 0, new Vector2(0, size.Y), Vector2.One * Screen.ScreenSize);

                text = "ボスのHP : " + BossHP;
                size = font.MeasureString(text);
                renderer.DrawString(font, text, new Vector2(0, Screen.HEIGHT - size.Y * 1f * Screen.ScreenSize), Color.White, 0, new Vector2(0, size.Y), Vector2.One * Screen.ScreenSize);

                text = "ボスのダメージカウント : " + DamageCount;
                size = font.MeasureString(text);
                renderer.DrawString(font, text, new Vector2(0, Screen.HEIGHT - size.Y * 2f * Screen.ScreenSize), Color.White, 0, new Vector2(0, size.Y), Vector2.One * Screen.ScreenSize);

                text = "ボスの状態 : " + BossState;
                size = font.MeasureString(text);
                renderer.DrawString(font, text, new Vector2(0, Screen.HEIGHT - size.Y * 3f * Screen.ScreenSize), Color.White, 0, new Vector2(0, size.Y), Vector2.One * Screen.ScreenSize);

                text = "isWait : " + isWait;
                size = font.MeasureString(text);
                renderer.DrawString(font, text, new Vector2(0, Screen.HEIGHT - size.Y * 4f * Screen.ScreenSize), Color.White, 0, new Vector2(0, size.Y), Vector2.One * Screen.ScreenSize);
            }
        }

        public override void HitAction(GameObject gameObject)
        {
            if (isWait || InvincibleFlag) { return; }

            if (gameObject.GameObjectTag == GameObjectTag.PlayerBullet)
            {
                BossHP -= 2;
                bariaDestRotateY += 90;
                isHeal = false;
                offsetDestScale = 5;
            }
        }

        void BossStateManage()
        {
            switch (BossState)
            {
                case BossStateEnum.Stage01:
                    Stage01();
                    break;
                case BossStateEnum.Stage02:
                    Stage02();
                    break;
                case BossStateEnum.Stage03:
                    Stage03();
                    break;
            }
        }

        void BossHPManage()
        {
            if (isHeal)
            {
                BossHP += 0.25f * Time.deltaSpeed;
            }
            else
            {
                healRebootTime += Time.deltaTime;
                if (healRebootTime >= healRebootLimit)
                {
                    healRebootTime = 0;
                    isHeal = true;
                }
            }

            BossHP = MathHelper.Clamp(BossHP, 0, 100);

            // ボスのHPが0なら
            if (BossHP <= 0)
            {
                isWait = true;

                AttackModules.Clear();
                MoveModule = new None_MM(this);

                // EnemyBulletを死亡させる
                ObjectsManager.RemoveEnemyBullet();

                // HPをリセット
                BossHP = 100;
                collscale = MIN_COLLSCALE;

                Time.BossBreakStop();
                Camera.Shake(2, 1, 0.95f);

                // ダメージカウントをインクリメント
                DamageCount++;
                DamageSumCount++;

                // 無敵状態へ
                InvincibleFlag = true;

                // もし3回やっつけたら次の状態へ
                if (DamageCount >= damageCountLimit)
                {
                    Time.TimeStop();

                    DamageCount = 0;
                    // もし最終段階だったら、ボスは死ぬ
                    if ((int)BossState == 3)
                    {
                        IsDead = true;
                    }
                    else
                    {
                        // 次の段階へ
                        BossState++;
                    }
                }
            }
        }

        void Stage01()
        {
            // 起動
            if (isWait)
            {
                waitTime += Time.deltaTime;
                if (waitTime >= waitLimit)
                {
                    isWait = false;
                    waitTime = 0;
                    AttackModules.Add(new CrossMine_AM(this));
                    AttackModules.Add(new Missile_AM(this));
                    AttackModules.Add(new AntiAir_AM(this));
                    MoveModule = new CircleRandom_MM(this);
                    HousyaAttack();
                }
            }
        }

        void Stage02()
        {
            // 起動
            if (isWait)
            {
                waitTime += Time.deltaTime;
                if (waitTime >= waitLimit)
                {
                    isWait = false;
                    waitTime = 0;
                    AttackModules.Add(new CrossMine_AM(this));
                    MoveModule = new CircleRandom_MM(this);
                    HousyaAttack();
                }
            }
        }

        void Stage03()
        {
            // 起動
            if (isWait)
            {
                waitTime += Time.deltaTime;
                if (waitTime >= waitLimit)
                {
                    isWait = false;
                    waitTime = 0;
                    AttackModules.Add(new CrossMine_AM(this));
                    MoveModule = new CircleRandom_MM(this);
                    HousyaAttack();
                }
            }
        }

        void HousyaAttack()
        {
            Random rand = GameDevice.Instance().Random;

            for (int i = 0; i < 360; i += 5)
            {
                Vector2 vec2 = MyMath.DegToVec2(i);
                float randY = MyMath.RandF(-1, 1) * 0.1f;
                Vector3 vec3 = new Vector3(vec2.Y, randY, vec2.X);
                ObjectsManager.AddGameObject(new Mine_Bullet(Position, vec3, rand.Next(50, 100) * 0.1f), false);
            }
        }

        void MoveParticle()
        {
            // 移動パーティクル
            fireTime += Time.deltaTime;

            if (fireTime >= fireLimit)
            {
                fireTime = 0;
                Random rand = GameDevice.Instance().Random;
                ObjectsManager.AddParticle(new BossRocketFire_Particle3D(Position - AngleVec3 * 50f, -AngleVec3 + MyMath.RandomCircleVec3() * 0.05f, rand));
            }
        }

        void ClampPosition()
        {
            // 位置を補正
            Vector3 offset = new Vector3(10, 0, 10);

            if (Position.X < offset.X || Position.Z < offset.Z || Position.X > ObjectsManager.MapLength - offset.X || Position.Z > ObjectsManager.MapLength - offset.Z)
            {
                MoveModule = new FirstPosition_MM(this);
            }

            Position = Vector3.Clamp(Position, Vector3.Zero + offset, new Vector3(ObjectsManager.MapLength, 0, ObjectsManager.MapLength) - offset);
        }

        void InvincibleManage()
        {
            if (InvincibleFlag)
            {
                invincibleTime += Time.deltaTime;

                color = (invincibleTime % 0.08f <= 0.04f) ? (Color.Black) : (Color.White);

                if (invincibleTime >= invincibleLimit)
                {
                    invincibleTime = 0;
                    InvincibleFlag = false;
                    color = Color.White;
                }
            }
        }
    }
}
