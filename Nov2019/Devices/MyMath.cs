using Microsoft.Xna.Framework;
using System;

namespace Nov2019.Devices
{
    static class MyMath
    {
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
    }
}
