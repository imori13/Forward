using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nov2019.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nov2019.Devices
{
    class Camera
    {
        public ObjectsManager ObjectsManager { get; set; }
        public Vector3 Position { get; set; }

        // private
        public Matrix View { get; private set; }    // カメラの位置、見る方向を元に計算するMatrix
        public Matrix Projection { get; private set; }  // カメラの視野範囲

        private Matrix rotationX;
        private Matrix rotationY;

        private GameDevice gameDevice;
        Random random;
        GameTime gameTime;

        float yaw;
        float pitch;

        Vector3 cameraPosAngleVec3;
        float cameraPosLatitude = 0;
        float cameraPosLongitude = 0;
        float zoom;

        private bool shaking;
        private float shakeMagnitude;
        private float shakeDuration;
        private float shakeDelay;
        private float shakeTimer;
        private Vector3 shakeOffset;

        Vector3 velocity;

        public Camera()
        {
            Position = Vector3.Zero;

            Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4, 16F / 9F, 0.001f, 100000f);

            yaw = MathHelper.ToRadians(180);
            pitch = MathHelper.ToRadians(-5);

            rotationX = Matrix.CreateRotationX(0);
            rotationY = Matrix.CreateRotationY(0);

            cameraPosLongitude = 0;
        }

        public void Update(Player player)
        {
            gameDevice = GameDevice.Instance();
            random = gameDevice.Random;
            gameTime = gameDevice.GameTime;

            View = CameraView(player);
            //View = Matrix.Lerp(View, DebugView(), 0.1f * Time.Speed);

            // マウスを画面の中心に置く
            Input.SetMousePosition(Screen.WIDTH / 2, Screen.HEIGHT / 2);
        }

        // 振動コピペURL
        // http://xnaessentials.com/post/2011/04/27/shake-that-camera.aspx

        Matrix CameraView(Player player)
        {
            float destZoom = 50f;
            if (player.PlayerRightClickMode)
            {
                destZoom = 30f;
                cameraPosLatitude = 10;
                cameraPosLongitude = MyMath.Vec2ToDeg(new Vector2(-player.AngleVec3.X, -player.AngleVec3.Z));
            }
            else
            {
                cameraPosLatitude += (Input.GetMousePosition().Y - Screen.HEIGHT / 2f) * 0.25f;
                cameraPosLatitude = MathHelper.Clamp(cameraPosLatitude, -20, 20);

                cameraPosLongitude += (Input.GetMousePosition().X - Screen.WIDTH / 2f) * 0.25f;
            }

            // カメラをプレイヤーの後ろに追従するように設定
            cameraPosAngleVec3 = Vector3.Lerp(
                    cameraPosAngleVec3,
                new Vector3(
                    (float)Math.Cos(MathHelper.ToRadians(cameraPosLongitude)),
                    (float)Math.Sin(MathHelper.ToRadians(cameraPosLatitude)),
                    (float)Math.Sin(MathHelper.ToRadians(cameraPosLongitude))),
                    1.0f * Time.Speed);

            zoom = MathHelper.Lerp(zoom, destZoom, 0.1f * Time.Speed);

            Position = Vector3.Lerp(Position, player.Position + (cameraPosAngleVec3 * zoom), 1.0f * Time.Speed);

            Vector3 viewPoint = player.Position;

            if (shaking)
            {
                shakeTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (shakeTimer >= shakeDuration)
                {
                    shaking = false;
                    shakeTimer = shakeDuration;
                }

                float progress = shakeTimer / shakeDuration;

                float magnitude = shakeMagnitude * (1f - (progress * progress));

                shakeOffset = new Vector3(NextFloat(), NextFloat(), NextFloat()) * magnitude;

                Position += shakeOffset;
                viewPoint += shakeOffset;

                shakeMagnitude *= shakeDelay;
            }

            // プレイヤーの方向をカメラが向く
            Matrix matrix = Matrix.Lerp(View, Matrix.CreateLookAt(Position, viewPoint, Vector3.Up), 0.1f * Time.Speed);

            return matrix;
        }

        Matrix DebugView()
        {
            // マウスが画面中央からどれだけ移動したかを取得し、値を変化させる
            // YawはY軸を中心とした横回転

            yaw -= (Input.GetMousePosition().X - Screen.WIDTH / 2) / 1000f; // ÷1000はマウスの速度 
            pitch -= (Input.GetMousePosition().Y - Screen.HEIGHT / 2) / 1000f; // PitchはX軸を中心とした縦回転

            // YawとPitchから前方の座標と左側、上側の座標を取得
            Vector3 forward = Vector3.TransformNormal(Vector3.Forward, Matrix.CreateFromYawPitchRoll(yaw, pitch, 0));
            Vector3 left = Vector3.TransformNormal(Vector3.Left, Matrix.CreateFromYawPitchRoll(yaw, pitch, 0));
            Vector3 up = Vector3.TransformNormal(Vector3.Up, Matrix.CreateFromYawPitchRoll(yaw, pitch, 0));

            Vector3 destVelocity = Vector3.Zero;

            // キー入力
            if (Input.GetKey(Keys.W)) destVelocity += forward;
            if (Input.GetKey(Keys.S)) destVelocity -= forward;
            if (Input.GetKey(Keys.A)) destVelocity += left;
            if (Input.GetKey(Keys.D)) destVelocity -= left;
            if (Input.GetKey(Keys.Space)) destVelocity += up;
            if (Input.GetKey(Keys.LeftControl)) destVelocity -= up;

            velocity = Vector3.Lerp(velocity, destVelocity * 0.5f, 0.25f);
            Position += velocity;

            // ビュー行列を作成
            return Matrix.Lerp(View, Matrix.CreateLookAt(Position, Position + forward, up), 0.1f * Time.Speed);
        }

        private float NextFloat()
        {
            return (float)random.NextDouble() * 2f - 1f;
        }

        public void Shake(float magnitude, float duration, float delay)
        {
            shaking = true;

            shakeMagnitude = magnitude;
            shakeDuration = duration;
            shakeDelay = delay;

            shakeTimer = 0f;
        }
    }
}
