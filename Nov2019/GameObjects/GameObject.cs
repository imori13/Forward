using Microsoft.Xna.Framework;
using Nov2019.Devices;
using Nov2019.Devices.Collision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nov2019.GameObjects
{
    // 抽象クラス
    abstract class GameObject
    {
        // 変数
        public Vector2 Position { get; set; } // 位置
        public Vector2 Velocity { get; protected set; } // 移動量 処理は各自必要な場合
        public GameObjectTag GameObjectTag { get; set; }  // ゲームタグ
        public Collider Collider { get; protected set; }
        public bool IsDead { get; protected set; }  // 死亡してるか
        public ObjectsManager ObjectsManager { get; set; }

        // コンストラクタ
        public GameObject()
        {

        }

        // 抽象メソッド
        public abstract void Initialize();  // 初期化
        public abstract void Update();  // 更新
        public abstract void Draw(Renderer renderer);
        public abstract void HitAction(GameObject gameObject);  // 衝突を検知した時に呼ばれる処理

        // "相手がボックスの時"のバウンド処理
        public void BoundBoxCollision(GameObject gameObject)
        {
            // 相手から見たオブジェクトの方向
            BoxCollider box = gameObject.Collider as BoxCollider;
            Vector2 nearPos = new Vector2(
                MathHelper.Clamp(Position.X, gameObject.Position.X - box.Size.X / 2f, gameObject.Position.X + box.Size.X / 2f),
                MathHelper.Clamp(Position.Y, gameObject.Position.Y - box.Size.Y / 2f, gameObject.Position.Y + box.Size.Y / 2f));
            Vector2 direction = Position - nearPos;

            if (direction.Length() != 0)
            {
                direction.Normalize();
            }

            int count = 0;

            if (direction != null)
            {
                while (Collider.IsCollision(gameObject.Collider))
                {
                    count++;
                    // 押し出す
                    Position += direction * 1f;

                    if (count > 100) { break; }
                }
            }

            // 押し出したら移動量を与える
            Velocity = direction * 50f;
        }

        // 相手が円コリジョンの時のバウンド処理
        public void BoundCircleCollision(GameObject gameobject)
        {
            Vector2 direction = Position - gameobject.Position;
            direction.Normalize();

            int count = 0;

            if (direction != null)
            {
                while (Collider.IsCollision(gameobject.Collider))
                {
                    count++;
                    // 押し出す
                    Position += direction * 1f;

                    if (count > 100) { break; }
                }
            }

            // 押し出したら移動量を与える
            Velocity = direction * 50f;
        }
    }
}
