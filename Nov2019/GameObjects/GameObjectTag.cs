using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nov2019.GameObjects
{
    enum GameObjectTag
    {
        None,   // タグなし
        Player, // プレイヤー
        BossEnemy,  // ボスエネミー
        Cube,
        PlayerBullet,   // プレイヤーの弾
        EnemyBullet,    // 敵の弾
        DamageCollision,
    }
}
