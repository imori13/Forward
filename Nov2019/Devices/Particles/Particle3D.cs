using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nov2019.Devices.Particles
{
    // エフェクトの基本パラメータを描いた、抽象クラス
    abstract class Particle3D: Particle
    {
        protected string modelName; // Modelのアセットの名前。Rendererに格納されてるDictionaryデータのKeyの名前
        protected Color modelColor; // モデルのカラー。textureとは共存できない。両方nullでないならtexture優先

        protected Vector3 position; // エフェクトの座標
        protected Vector3 direction; // エフェクトの移動する角度  度数->Vector2にしたいならStaticのCalculationクラスのAngleToVelocityを使うか同じ処理書いて。
        protected float speed;  // エフェクトの移動する際の移動量の速度。 velocity*speed 
        protected float friction;   // エフェクトの移動する際の摩擦 speed*frictionする。0.95fとかそういう値

        protected Vector3 scale;  // エフェクトの拡縮値

        protected Vector3 rotation;   // エフェクトの回転値(度数) 描画時に親でラジアンに変換してるので気にせず度数法のまま使ってOK
        protected Vector3 rotation_rotate; // 死亡時にどれくらい回転するか。rotation=180でdest_rotation90なら、生成時180,死亡時270まで回転する
        protected Vector3 origin;   // エフェクトの回転するときの中心 (50px*50pxだったら,Vec2(25,25)みたいな)

        protected float aliveTime;  // 生成時から時間を数える変数
        protected float aliveLimit; // 生存時間。この時間だけ生きる。という意味。値が5なら、5秒たったら死ぬ
        protected float aliveRate;  // 現在経った時間と、生存時間を割ったレート。生成時は0、死亡時は1。どんどん0から1に向かう感じ

        private GameDevice gameDevice;
        private Vector3 rotation_dest;    // rotation+rotation_rotateの合計値。線形補完の時に使う。
        private Vector3 rotation_start;   // rotationの最初の値を保存。線形補完の時に使う。

        public Particle3D(
           string modelName,
           Color modelColor,
           float aliveLimit,
           Vector3 position,
           Vector3 direction,
           float speed,
           float friction,
           Vector3 scale,
           Vector3 rotation,
           Vector3 rotation_speed,
           Vector3 origin)
        {
            this.modelName = modelName;
            this.modelColor = modelColor;
            this.aliveLimit = aliveLimit;
            this.position = position;
            this.direction = direction;
            this.speed = speed;
            this.friction = friction;
            this.scale = scale;
            this.rotation = rotation;
            this.rotation_rotate = rotation_speed;
            this.origin = origin;

            IsDead = false;
            gameDevice = GameDevice.Instance();
        }

        public override void Initialize()
        {
            rotation_dest = rotation + rotation_rotate;
            rotation_start = rotation;
        }

        // overrideした子クラスでは最後に呼び出す。
        public override void Update()
        {
            aliveTime += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds * Time.Speed;  // 時間数えてる
            aliveRate = aliveTime / aliveLimit; // レート=現在時間/生存時間  0.25=1/4 4秒のエフェクトでいま1秒なら0.25
            if (aliveTime > aliveLimit) // 生存時間超えたらしぬ
            {
                IsDead = true;
            }

            // 回転速度を回転パラメータにぶち込み続ける
            rotation = Vector3.Lerp(rotation_start, rotation_dest, GetAliveRate());

            // 速度*摩擦 どんどん移動速度が落ちるとかに
            // TimeSpeedの係数を受け入れられる計算式
            speed -= (speed - (speed * friction)) * Time.Speed;

            // 座標=移動角度*速度
            position += direction * speed * Time.Speed;
        }

        public override void Draw(Renderer renderer, Camera camera)
        {
            Matrix world =
                Matrix.CreateScale(scale) *
                Matrix.CreateRotationX(MathHelper.ToRadians(rotation.X)) *
                Matrix.CreateRotationY(MathHelper.ToRadians(rotation.Y)) *
                Matrix.CreateRotationZ(MathHelper.ToRadians(rotation.Z)) *
                Matrix.CreateWorld(position + origin, Vector3.Forward, Vector3.Up);

            renderer.Draw3D(
            modelName,
            modelColor,
            camera,
            world);
        }

        public float GetAliveRate()
        {
            return aliveRate;
        }
    }
}