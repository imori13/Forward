using Microsoft.Xna.Framework;

namespace Nov2019.Devices.Particles
{
    // エフェクトの基本パラメータを描いた、抽象クラス
    abstract class Particle2D : Particle
    {
        protected string assetName; // Rendererに格納されてるDictionaryデータのKeyの名前

        protected Vector2 position; // エフェクトの座標
        protected Vector2 direction; // エフェクトの移動する角度  度数->Vector2にしたいならStaticのCalculationクラスのAngleToVelocityを使うか同じ処理書いて。
        protected float speed;  // エフェクトの移動する際の移動量の速度。 velocity*speed 
        protected float friction;   // エフェクトの移動する際の摩擦 speed*frictionする。0.95fとかそういう値

        protected Color color;  // エフェクトの色
        protected float alpha;  // エフェクトの透明値

        protected Vector2 scale;  // エフェクトの拡縮値

        protected float rotation;   // エフェクトの回転値(度数) 描画時に親でラジアンに変換してるので気にせず度数法のまま使ってOK
        protected float rotation_speed; // 死亡時にどれくらい回転するか。rotation=180でdest_rotation90なら、生成時180,死亡時270まで回転する
        protected Vector2 origin;   // エフェクトの回転するときの中心 (50px*50pxだったら,Vec2(25,25)みたいな)

        protected float aliveTime;  // 生成時から時間を数える変数
        protected float aliveLimit; // 生存時間。この時間だけ生きる。という意味。値が5なら、5秒たったら死ぬ
        protected float aliveRate;  // 現在経った時間と、生存時間を割ったレート。生成時は0、死亡時は1。どんどん0から1に向かう感じ

        protected GameDevice gameDevice;
        protected float rotation_dest;    // rotation+rotation_rotateの合計値。線形補完の時に使う。
        protected float rotation_start;   // rotationの最初の値を保存。線形補完の時に使う。

        float frictionTime;
        float frictionLimit = 0.025f;

        // 引数一番多い版。少ないやつをオーバーロードメソッドとして新しく作っちゃってもオッケー 
        // 同じ名前のメソッドでも引数が違うメソッドが書けるのがオーバーロード。
        public Particle2D(
            string assetName,
            float aliveLimit,
            Vector2 position,
            Vector2 direction,
            float speed,
            float friction,
            Color color,
            float alpha,
            Vector2 scale,
            float rotation,
            float rotation_speed,
            Vector2 origin)
        {
            this.assetName = assetName;
            this.aliveLimit = aliveLimit;
            this.position = position;
            this.direction = direction;
            this.speed = speed;
            this.friction = friction;
            this.color = color;
            this.alpha = alpha;
            this.scale = scale;
            this.rotation = rotation;
            this.rotation_speed = rotation_speed;
            this.origin = origin;

            IsDead = false;
            gameDevice = GameDevice.Instance();
        }

        public override void Initialize()
        {
            rotation_dest = rotation + rotation_speed;
            rotation_start = rotation;
        }

        // overrideした子クラスでは最後に呼び出す。
        public override void Update()
        {
            aliveTime += (float)gameDevice.GameTime.ElapsedGameTime.TotalSeconds * Time.Speed;  // 時間数えてる
            aliveRate = aliveTime / aliveLimit; // レート=現在時間/生存時間  0.25=1/4 4秒のエフェクトでいま1秒なら0.25
            if (aliveTime > aliveLimit) // 生存時間超えたらしぬ
            {
                IsDead = true;
            }

            // 回転速度を回転パラメータにぶち込み続ける
            rotation = MathHelper.Lerp(rotation_start, rotation_dest, GetAliveRate());

            // 速度*摩擦 どんどん移動速度が落ちるとかに
            frictionTime += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds * Time.Speed;
            if (frictionTime >= frictionLimit)
            {
                frictionTime = 0;
                speed *= friction;
            }

            // 座標=移動角度*速度
            position += direction * speed * Time.Speed;
        }

        public override void Draw(Renderer renderer, Camera camera)
        {
            renderer.Draw2D(
                assetName,
                position,
                color,
                MathHelper.ToRadians(rotation),
                origin,
                scale * Screen.ScreenSize);
        }

        public float GetAliveRate()
        {
            return aliveRate;
        }
    }
}
