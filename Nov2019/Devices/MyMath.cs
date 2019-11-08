using Microsoft.Xna.Framework;
using System;

namespace Nov2019.Devices
{
    static class MyMath
    {
        static Random rand = GameDevice.Instance().Random;

        // 度数角度をVector2に変換する
        public static Vector2 DegToVec2(float kakudo)
        {
            return new Vector2((float)Math.Cos(MathHelper.ToRadians(kakudo)), (float)Math.Sin(MathHelper.ToRadians(kakudo)));
        }

        // Vector2のベクトルをラジアン角度に変換する
        public static float Vec2ToDeg(Vector2 vec2)
        {
            return MathHelper.ToDegrees((float)Math.Atan2(vec2.Y, vec2.X));
        }

        // 0~360のランダムな角度のVector2を返す
        public static Vector2 RandomCircleVec2()
        {
            Random rand = GameDevice.Instance().Random;
            float radian = MathHelper.ToRadians(rand.Next(360) + (float)rand.NextDouble());

            return new Vector2((float)Math.Cos(radian), (float)Math.Sin(radian));
        }

        public static Vector3 SphereVec3(float latitude, float longitude)
        {
            float x = (float)(Math.Cos(latitude) * Math.Cos(longitude));
            float y = (float)(Math.Sin(latitude));
            float z = (float)(Math.Cos(latitude) * Math.Sin(longitude));

            return new Vector3(x, y, z);
        }

        public static Vector3 RandomCircleVec3()
        {
            Vector3 value = new Vector3(RandF(-1, 1), RandF(-1, 1), RandF(-1, 1));
            value.Normalize();
            return value;
        }

        public static float RandF(float min, float max)
        {
            return rand.Next((int)min, (int)max) + (float)rand.NextDouble();
        }

        public static float RandF(float max)
        {
            return RandF(0, max);
        }
    }
}
