using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nov2019.Devices
{
    class Camera
    {
        public Vector2 Position { get; set; }
        public Vector2 Zoom { get; set; }
        public float Rotation { get; set; }

        private bool shaking;
        private float shakeMagnitude;
        private float shakeDuration;
        private float shakeDelay;
        private float shakeTimer;
        private Vector2 shakeOffset;

        Random rand = GameDevice.Instance().Random;

        public Matrix Matrix
        {
            get
            {
                if (shaking)
                {
                    shakeTimer += (float)GameDevice.Instance().GameTime.ElapsedGameTime.TotalSeconds;

                    if (shakeTimer >= shakeDuration)
                    {
                        shaking = false;
                        shakeTimer = shakeDuration;
                    }

                    float progress = shakeTimer / shakeDuration;

                    float magnitude = shakeMagnitude * (1f - (progress * progress));

                    shakeOffset = new Vector2(NextFloat(), NextFloat()) * magnitude;

                    Position += shakeOffset;

                    shakeMagnitude *= shakeDelay;
                }

                Vector3 pos = new Vector3(Position, 0);
                Vector3 screenPos = new Vector3(Screen.WIDTH / 2f, Screen.HEIGHT / 2f, 0);

                return Matrix.CreateTranslation(-pos) *
                       Matrix.CreateScale(new Vector3(Zoom, 0)) *
                       Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation)) *
                       Matrix.CreateTranslation(screenPos);

            }
        }

        public Camera()
        {
            Zoom = Vector2.One;
        }

        public void Initialize()
        {
            Position = Vector2.Zero;
        }

        public void Update()
        {

        }

        private float NextFloat()
        {
            return (float)rand.NextDouble() * 2f - 1f;
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
