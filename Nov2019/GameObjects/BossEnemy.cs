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
using Nov2019.GameObjects.AttackModuleManages;
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

        public float DestAngle { get; set; }
        public float Angle { get; private set; }    // 向く角度

        public AttackModuleManage AttackModuleManage { get; private set; }

        float fireTime;
        float fireLimit = 0.05f;

        // 最初の待機状態の変数
        float waitTime;
        static readonly float waitLimit = 2.5f;
        bool isWait;
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
        public int DamageCount { get; private set; }    // HPが0になった回数
        static readonly int damageCountLimit = 3;

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
            AttackModuleManage = new Stage01AMManage(this);
            AttackModuleManage.NoneModule();

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
            AttackModuleManage.Update();

            BossHPManage();

            MoveParticle();

            float rate = BossHP / 100f;
            collscale = MathHelper.Lerp(collscale, Easing2D.ExpOut(BossHP, 100, MAX_COLLSCALE, MIN_COLLSCALE), 0.25f * Time.deltaSpeed);
            Collider = new CircleCollider(this, collscale);

            bariaAlpha = (isWait) ? (0) : (MathHelper.Lerp(bariaAlpha, 0.5f, 0.1f * Time.deltaSpeed));

            color = Color.Lerp(color, (isWait) ? (Color.Gray) : (Color.White), 0.1f * Time.deltaSpeed);

            bariaRotateY = MathHelper.Lerp(bariaRotateY, bariaDestRotateY, 0.1f * Time.deltaSpeed);
            offsetDestScale = MathHelper.Lerp(offsetDestScale, 0, 0.1f * Time.deltaSpeed);
            offsetScale = MathHelper.Lerp(offsetScale, offsetDestScale, 0.5f * Time.deltaSpeed);

            Angle = MathHelper.Lerp(Angle, DestAngle, 0.05f * Time.deltaSpeed);
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

            renderer.Draw3D("boat", "boat_blue", color, Camera, world);

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

            float distance = Vector3.Distance(Position, ObjectsManager.Player.Position);
            text = "プレイヤーとボスの距離 : " + distance.ToString("000.00");
            size = font.MeasureString(text);
            renderer.DrawString(font, text, new Vector2(0, Screen.HEIGHT - size.Y * 0f), Color.White, 0, new Vector2(0, size.Y), Vector2.One);

            text = "ボスのHP : " + BossHP;
            size = font.MeasureString(text);
            renderer.DrawString(font, text, new Vector2(0, Screen.HEIGHT - size.Y * 1f), Color.White, 0, new Vector2(0, size.Y), Vector2.One);

            text = "ボスのダメージカウント : " + DamageCount;
            size = font.MeasureString(text);
            renderer.DrawString(font, text, new Vector2(0, Screen.HEIGHT - size.Y * 2f), Color.White, 0, new Vector2(0, size.Y), Vector2.One);

            text = "ボスの状態 : " + BossState;
            size = font.MeasureString(text);
            renderer.DrawString(font, text, new Vector2(0, Screen.HEIGHT - size.Y * 3f), Color.White, 0, new Vector2(0, size.Y), Vector2.One);

            text = "isWait : " + isWait;
            size = font.MeasureString(text);
            renderer.DrawString(font, text, new Vector2(0, Screen.HEIGHT - size.Y * 4f), Color.White, 0, new Vector2(0, size.Y), Vector2.One);
        }

        public override void HitAction(GameObject gameObject)
        {
            if (isWait) { return; }

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

                AttackModuleManage.NoneModule();

                // EnemyBulletを死亡させる
                ObjectsManager.RemoveEnemyBullet();

                // HPをリセット
                BossHP = 100;
                collscale = MIN_COLLSCALE;

                // ダメージカウントをインクリメント
                DamageCount++;

                // もし3回やっつけたら次の状態へ
                if (DamageCount >= damageCountLimit)
                {
                    Time.StopTime();

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

                        switch (BossState)
                        {
                            case BossStateEnum.Stage02:
                                AttackModuleManage = new Stage02AMManage(this);
                                break;
                            case BossStateEnum.Stage03:
                                AttackModuleManage = new Stage03AMManage(this);
                                break;
                        }
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
                    AttackModuleManage.ChangeModule();
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
                    AttackModuleManage.ChangeModule();
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
                    AttackModuleManage.ChangeModule();
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
                ObjectsManager.AddGameObject(new Normal_Bullet(Position, vec3, rand.Next(50, 100) * 0.01f), false);
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

            float forcePower = 0.5f;
            if (Position.X < offset.X)
            {
                Velocity = new Vector3(forcePower, 0, Velocity.Z);
            }
            else if (Position.Z < offset.Z)
            {
                Velocity = new Vector3(Velocity.X, 0, forcePower);
            }
            else if (Position.X > ObjectsManager.MapLength - offset.X)
            {
                Velocity = new Vector3(-forcePower, 0, Velocity.Z);
            }
            else if (Position.Z > ObjectsManager.MapLength - offset.Z)
            {
                Velocity = new Vector3(Velocity.X, 0, -forcePower);
            }

            Position = Vector3.Clamp(Position, Vector3.Zero + offset, new Vector3(ObjectsManager.MapLength, 0, ObjectsManager.MapLength) - offset);
        }
    }
}
