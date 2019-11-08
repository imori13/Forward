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
        public uint CurrentRootPos { get; set; } = uint.MaxValue;   // 空間分割したなかのどの空間にいるのか
        public Vector3 Position { get; set; } // 位置
        public Vector3 Velocity { get; protected set; } // 移動量 処理は各自必要な場合
        public GameObjectTag GameObjectTag { get; set; }  // ゲームタグ
        public Collider Collider { get; protected set; }
        public bool IsDead { get; protected set; }  // 死亡してるか
        public ObjectsManager ObjectsManager { get; set; }
        public Camera Camera { get; set; }

        // コンストラクタ
        public GameObject()
        {

        }

        // 抽象メソッド
        public abstract void Initialize();  // 初期化
        public abstract void Update();  // 更新
        public abstract void Draw(Renderer renderer);
        public abstract void DrawUI(Renderer renderer);
        public abstract void HitAction(GameObject gameObject);  // 衝突を検知した時に呼ばれる処理

        // "相手がボックスの時"のバウンド処理
        public void BoundBoxCollision(GameObject gameObject)
        {
            // 相手から見たオブジェクトの方向
            BoxCollider box = gameObject.Collider as BoxCollider;
            Vector3 nearPos = new Vector3(
                MathHelper.Clamp(Position.X, gameObject.Position.X - box.Size.X / 2f, gameObject.Position.X + box.Size.X / 2f),
                MathHelper.Clamp(Position.Y, gameObject.Position.Y - box.Size.Y / 2f, gameObject.Position.Y + box.Size.Y / 2f),
                MathHelper.Clamp(Position.Z, gameObject.Position.Y - box.Size.Z / 2f, gameObject.Position.Z + box.Size.Z / 2f));
            Vector3 direction = Position - nearPos;

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
            Vector3 direction = Position - gameobject.Position;
            direction.Normalize();

            int count = 0;

            if (direction != null)
            {
                while (Collider.IsCollision(gameobject.Collider))
                {
                    count++;
                    // 押し出す
                    Position += direction * 0.1f;

                    if (count > 100) { break; }
                }
            }

            // 押し出したら移動量を与える
            Velocity = ((direction * gameobject.Velocity.Length()) + gameobject.Velocity * 0.25f) * 0.5f;
        }

        // 当たり判定を行う空間を更新するメソッド
        // 動いたら呼び出すように
        public void UpdateListPos()
        {
            if (Collider == null) { return; }


            if (Position.X < 0 || Position.X >= ObjectsManager.MapLength || Position.Z < 0 || Position.Z >= ObjectsManager.MapLength)
            {
                IsDead = true;
                return;
            }

            float length = ObjectsManager.unitLength;
            Vector3 MinPos = new Vector3(0, 0, 0);
            Vector3 MaxPos = new Vector3(0, 0, 0);
            if (Collider.ColliderEnum == ColliderEnum.Circle)
            {
                CircleCollider collidert = (CircleCollider)Collider;
                MinPos = new Vector3(Position.X - collidert.Radius, 0, Position.Z - collidert.Radius);
                MaxPos = new Vector3(Position.X + collidert.Radius, 0, Position.Z + collidert.Radius);
            }
            else if (Collider.ColliderEnum == ColliderEnum.Box)
            {
                BoxCollider a = (BoxCollider)Collider;
                MinPos = new Vector3(Position.X - a.Size.X, 0, Position.Z - a.Size.Z);
                MaxPos = new Vector3(Position.X + a.Size.X, 0, Position.Z + a.Size.Z);
            }

            // 20=孫空間までの配列サイズ
            // 空間の開始位置　= 4の空間分割乗(ルートなら0,親なら1,子なら2,孫3)-1 / 3
            // L = (4*4*4)-1 / 3 = 21 ←配列での孫空間の開始位置
            uint newRootPos = XYtoMotton((ushort)(Position.X * length), (ushort)(Position.Z * length));

            uint p1 = XYtoMotton((ushort)(MinPos.X * length), (ushort)(MinPos.Z * length));
            uint p2 = XYtoMotton((ushort)(MaxPos.X * length), (ushort)(MaxPos.Z * length));
            uint checkXOR = p1 ^ p2;

            // ルート・親・子・孫　どの空間に所属するかの処理
            // サイズが大きかったり、空間をまたがっているやつは上の階層に送られる

            // ルート空間
            if ((checkXOR & 48) != 0)
            {
                newRootPos = 0;
            }
            // 親空間
            else if ((checkXOR & 12) != 0)
            {
                newRootPos = (p1 >> 4) + 1;
            }
            // 子空間
            else if ((checkXOR & 3) != 0)
            {
                newRootPos = (p1 >> 2) + 5;
            }
            else
            {
                newRootPos += 21;
            }

            if (ObjectsManager.Array.Length <= newRootPos || newRootPos < 0)
            {
                IsDead = true;

                return;
            }

            if (CurrentRootPos == newRootPos)
            {
                return;
            }

            // 初期値をMaxValueにしてるので、初期値じゃなければ削除
            if (CurrentRootPos != newRootPos && CurrentRootPos != uint.MaxValue)
            {
                // ここ重いかも
                ObjectsManager.Array[CurrentRootPos].RemoveAt(ObjectsManager.Array[CurrentRootPos].IndexOf(this));
            }
            CurrentRootPos = newRootPos;

            ObjectsManager.Array[newRootPos].Add(this);
        }

        public static uint XYtoMotton(ushort x, ushort y)
        {
            return (BitSeparate32(x) | (BitSeparate32(y) << 1));
        }

        public static uint BitSeparate32(ushort n)
        {
            uint i = (uint)n;
            i = (i | (i << 8)) & 0x00ff00ff;
            i = (i | (i << 4)) & 0x0f0f0f0f;
            i = (i | (i << 2)) & 0x33333333;
            return (i | (i << 1)) & 0x55555555;
        }
    }
}
