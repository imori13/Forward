using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nov2019.Devices
{
    public class Easing2D
    {
        public static float QuadIn(float t, float totaltime, float min, float max)
        {
            max -= min;
            t /= totaltime;
            return max * t * t + min;
        }

        public static float QuadOut(float t, float totaltime, float min, float max)
        {
            max -= min;
            t /= totaltime;
            return -max * t * (t - 2) + min;
        }

        public static float QuadInOut(float t, float totaltime, float min, float max)
        {
            max -= min;
            t /= totaltime / 2;
            if (t < 1) return max / 2 * t * t + min;

            t = t - 1;
            return -max / 2 * (t * (t - 2) - 1) + min;
        }

        public static float CubicIn(float t, float totaltime, float min, float max)
        {
            max -= min;
            t /= totaltime;
            return max * t * t * t + min;
        }

        public static float CubicOut(float t, float totaltime, float min, float max)
        {
            max -= min;
            t = t / totaltime - 1;
            return max * (t * t * t + 1) + min;
        }

        public static float CubicInOut(float t, float totaltime, float min, float max)
        {
            max -= min;
            t /= totaltime / 2;
            if (t < 1) return max / 2 * t * t * t + min;

            t = t - 2;
            return max / 2 * (t * t * t + 2) + min;
        }

        public static float QuartIn(float t, float totaltime, float min, float max)
        {
            max -= min;
            t /= totaltime;
            return max * t * t * t * t + min;
        }

        public static float QuartOut(float t, float totaltime, float min, float max)
        {
            max -= min;
            t = t / totaltime - 1;
            return -max * (t * t * t * t - 1) + min;
        }

        public static float QuartInOut(float t, float totaltime, float min, float max)
        {
            max -= min;
            t /= totaltime / 2;
            if (t < 1) return max / 2 * t * t * t * t + min;

            t = t - 2;
            return -max / 2 * (t * t * t * t - 2) + min;
        }

        public static float QuintIn(float t, float totaltime, float min, float max)
        {
            max -= min;
            t /= totaltime;
            return max * t * t * t * t * t + min;
        }

        public static float QuintOut(float t, float totaltime, float min, float max)
        {
            max -= min;
            t = t / totaltime - 1;
            return max * (t * t * t * t * t + 1) + min;
        }

        public static float QuintInOut(float t, float totaltime, float min, float max)
        {
            max -= min;
            t /= totaltime / 2;
            if (t < 1) return max / 2 * t * t * t * t * t + min;

            t = t - 2;
            return max / 2 * (t * t * t * t * t + 2) + min;
        }

        public static float SineIn(float t, float totaltime, float min, float max)
        {
            max -= min;
            return -max * (float)Math.Cos(t * (Math.PI * 90 / 180) / totaltime) + max + min;
        }

        public static float SineOut(float t, float totaltime, float min, float max)
        {
            max -= min;
            return max * (float)Math.Sin(t * (Math.PI * 90 / 180) / totaltime) + min;
        }

        public static float SineInOut(float t, float totaltime, float min, float max)
        {
            max -= min;
            return -max / 2 * ((float)Math.Cos(t * Math.PI / totaltime) - 1) + min;
        }

        public static float ExpIn(float t, float totaltime, float min, float max)
        {
            max -= min;
            return t == 0.0 ? min : max * (float)Math.Pow(2, 10 * (t / totaltime - 1)) + min;
        }

        public static float ExpOut(float t, float totaltime, float min, float max)
        {
            max -= min;
            return t == totaltime ? max + min : max * (-(float)Math.Pow(2, -10 * t / totaltime) + 1) + min;
        }

        public static float ExpInOut(float t, float totaltime, float min, float max)
        {
            if (t == 0.0f) return min;
            if (t == totaltime) return max;
            max -= min;
            t /= totaltime / 2;

            if (t < 1) return max / 2 * (float)Math.Pow(2, 10 * (t - 1)) + min;

            t = t - 1;
            return max / 2 * (-(float)Math.Pow(2, -10 * t) + 2) + min;

        }

        public static float CircIn(float t, float totaltime, float min, float max)
        {
            max -= min;
            t /= totaltime;
            return -max * ((float)Math.Sqrt(1 - t * t) - 1) + min;
        }

        public static float CircOut(float t, float totaltime, float min, float max)
        {
            max -= min;
            t = t / totaltime - 1;
            return max * (float)Math.Sqrt(1 - t * t) + min;
        }

        public static float CircInOut(float t, float totaltime, float min, float max)
        {
            max -= min;
            t /= totaltime / 2;
            if (t < 1) return -max / 2 * ((float)Math.Sqrt(1 - t * t) - 1) + min;

            t = t - 2;
            return max / 2 * ((float)Math.Sqrt(1 - t * t) + 1) + min;
        }

        public static float ElasticIn(float t, float totaltime, float min, float max)
        {
            max -= min;
            t /= totaltime;

            float s = 1.70158f;
            float p = totaltime * 0.3f;
            float a = max;

            if (t == 0) return min;
            if (t == 1) return min + max;

            if (a < Math.Abs(max))
            {
                a = max;
                s = p / 4;
            }
            else
            {
                s = p / (2 * (float)Math.PI) * (float)Math.Asin(max / a);
            }

            t = t - 1;
            return -(a * (float)Math.Pow(2, 10 * t) * (float)Math.Sin((t * totaltime - s) * (2 * Math.PI) / p)) + min;
        }

        public static float ElasticOut(float t, float totaltime, float min, float max)
        {
            max -= min;
            t /= totaltime;

            float s = 1.70158f;
            float p = totaltime * 0.3f; ;
            float a = max;

            if (t == 0) return min;
            if (t == 1) return min + max;

            if (a < Math.Abs(max))
            {
                a = max;
                s = p / 4;
            }
            else
            {
                s = p / (2 * (float)Math.PI) * (float)Math.Asin(max / a);
            }

            return a * (float)Math.Pow(2, -10 * t) * (float)Math.Sin((t * totaltime - s) * (2 * (float)Math.PI) / p) + max + min;
        }

        public static float ElasticInOut(float t, float totaltime, float min, float max)
        {
            max -= min;
            t /= totaltime / 2;

            float s = 1.70158f;
            float p = totaltime * (0.3f * 1.5f);
            float a = max;

            if (t == 0) return min;
            if (t == 2) return min + max;

            if (a < Math.Abs(max))
            {
                a = max;
                s = p / 4;
            }
            else
            {
                s = p / (2 * (float)Math.PI) * (float)Math.Asin(max / a);
            }

            if (t < 1)
            {
                return -0.5f * (a * (float)Math.Pow(2, 10 * (t -= 1)) * (float)Math.Sin((t * totaltime - s) * (2 * Math.PI) / p)) + min;
            }

            t = t - 1;
            return a * (float)Math.Pow(2, -10 * t) * (float)Math.Sin((t * totaltime - s) * (2 * (float)Math.PI) / p) * 0.5f + max + min;
        }

        public static float BackIn(float t, float totaltime, float min, float max, float s)
        {
            max -= min;
            t /= totaltime;
            return max * t * t * ((s + 1) * t - s) + min;
        }

        public static float BackOut(float t, float totaltime, float min, float max, float s)
        {
            max -= min;
            t = t / totaltime - 1;
            return max * (t * t * ((s + 1) * t + s) + 1) + min;
        }

        public static float BackInOut(float t, float totaltime, float min, float max, float s)
        {
            max -= min;
            s *= 1.525f;
            t /= totaltime / 2;
            if (t < 1) return max / 2 * (t * t * ((s + 1) * t - s)) + min;

            t = t - 2;
            return max / 2 * (t * t * ((s + 1) * t + s) + 2) + min;
        }

        public static float BounceIn(float t, float totaltime, float min, float max)
        {
            max -= min;
            return max - BounceOut(totaltime - t, totaltime, 0, max) + min;
        }

        public static float BounceOut(float t, float totaltime, float min, float max)
        {
            max -= min;
            t /= totaltime;

            if (t < 1.0f / 2.75f)
            {
                return max * (7.5625f * t * t) + min;
            }
            else if (t < 2.0f / 2.75f)
            {
                t -= 1.5f / 2.75f;
                return max * (7.5625f * t * t + 0.75f) + min;
            }
            else if (t < 2.5f / 2.75f)
            {
                t -= 2.25f / 2.75f;
                return max * (7.5625f * t * t + 0.9375f) + min;
            }
            else
            {
                t -= 2.625f / 2.75f;
                return max * (7.5625f * t * t + 0.984375f) + min;
            }
        }

        public static float BounceInOut(float t, float totaltime, float min, float max)
        {
            if (t < totaltime / 2)
            {
                return BounceIn(t * 2, totaltime, 0, max - min) * 0.5f + min;
            }
            else
            {
                return BounceOut(t * 2 - totaltime, totaltime, 0, max - min) * 0.5f + min + (max - min) * 0.5f;
            }
        }

        public static float Linear(float t, float totaltime, float min, float max)
        {
            return (max - min) * t / totaltime + min;
        }

        public static Vector2 QuadIn(float t, float totaltime, Vector2 min, Vector2 max)
        {
            max -= min;
            t /= totaltime;
            return max * t * t + min;
        }

        public static Vector2 QuadOut(float t, float totaltime, Vector2 min, Vector2 max)
        {
            max -= min;
            t /= totaltime;
            return -max * t * (t - 2) + min;
        }

        public static Vector2 QuadInOut(float t, float totaltime, Vector2 min, Vector2 max)
        {
            max -= min;
            t /= totaltime / 2;
            if (t < 1) return max / 2 * t * t + min;

            t = t - 1;
            return -max / 2 * (t * (t - 2) - 1) + min;
        }

        public static Vector2 CubicIn(float t, float totaltime, Vector2 min, Vector2 max)
        {
            max -= min;
            t /= totaltime;
            return max * t * t * t + min;
        }

        public static Vector2 CubicOut(float t, float totaltime, Vector2 min, Vector2 max)
        {
            max -= min;
            t = t / totaltime - 1;
            return max * (t * t * t + 1) + min;
        }

        public static Vector2 CubicInOut(float t, float totaltime, Vector2 min, Vector2 max)
        {
            max -= min;
            t /= totaltime / 2;
            if (t < 1) return max / 2 * t * t * t + min;

            t = t - 2;
            return max / 2 * (t * t * t + 2) + min;
        }

        public static Vector2 QuartIn(float t, float totaltime, Vector2 min, Vector2 max)
        {
            max -= min;
            t /= totaltime;
            return max * t * t * t * t + min;
        }

        public static Vector2 QuartOut(float t, float totaltime, Vector2 min, Vector2 max)
        {
            max -= min;
            t = t / totaltime - 1;
            return -max * (t * t * t * t - 1) + min;
        }

        public static Vector2 QuartInOut(float t, float totaltime, Vector2 min, Vector2 max)
        {
            max -= min;
            t /= totaltime / 2;
            if (t < 1) return max / 2 * t * t * t * t + min;

            t = t - 2;
            return -max / 2 * (t * t * t * t - 2) + min;
        }

        public static Vector2 QuintIn(float t, float totaltime, Vector2 min, Vector2 max)
        {
            max -= min;
            t /= totaltime;
            return max * t * t * t * t * t + min;
        }

        public static Vector2 QuintOut(float t, float totaltime, Vector2 min, Vector2 max)
        {
            max -= min;
            t = t / totaltime - 1;
            return max * (t * t * t * t * t + 1) + min;
        }

        public static Vector2 QuintInOut(float t, float totaltime, Vector2 min, Vector2 max)
        {
            max -= min;
            t /= totaltime / 2;
            if (t < 1) return max / 2 * t * t * t * t * t + min;

            t = t - 2;
            return max / 2 * (t * t * t * t * t + 2) + min;
        }

        public static Vector2 SineIn(float t, float totaltime, Vector2 min, Vector2 max)
        {
            max -= min;
            return -max * (float)Math.Cos(t * (Math.PI * 90 / 180) / totaltime) + max + min;
        }

        public static Vector2 SineOut(float t, float totaltime, Vector2 min, Vector2 max)
        {
            max -= min;
            return max * (float)Math.Sin(t * (Math.PI * 90 / 180) / totaltime) + min;
        }

        public static Vector2 SineInOut(float t, float totaltime, Vector2 min, Vector2 max)
        {
            max -= min;
            return -max / 2 * ((float)Math.Cos(t * Math.PI / totaltime) - 1) + min;
        }

        public static Vector2 ExpIn(float t, float totaltime, Vector2 min, Vector2 max)
        {
            max -= min;
            return t == 0.0 ? min : max * (float)Math.Pow(2, 10 * (t / totaltime - 1)) + min;
        }

        public static Vector2 ExpOut(float t, float totaltime, Vector2 min, Vector2 max)
        {
            max -= min;
            return t == totaltime ? max + min : max * (-(float)Math.Pow(2, -10 * t / totaltime) + 1) + min;
        }

        public static Vector2 ExpInOut(float t, float totaltime, Vector2 min, Vector2 max)
        {
            if (t == 0.0f) return min;
            if (t == totaltime) return max;
            max -= min;
            t /= totaltime / 2;

            if (t < 1) return max / 2 * (float)Math.Pow(2, 10 * (t - 1)) + min;

            t = t - 1;
            return max / 2 * (-(float)Math.Pow(2, -10 * t) + 2) + min;

        }

        public static Vector2 CircIn(float t, float totaltime, Vector2 min, Vector2 max)
        {
            max -= min;
            t /= totaltime;
            return -max * ((float)Math.Sqrt(1 - t * t) - 1) + min;
        }

        public static Vector2 CircOut(float t, float totaltime, Vector2 min, Vector2 max)
        {
            max -= min;
            t = t / totaltime - 1;
            return max * (float)Math.Sqrt(1 - t * t) + min;
        }

        public static Vector2 CircInOut(float t, float totaltime, Vector2 min, Vector2 max)
        {
            max -= min;
            t /= totaltime / 2;
            if (t < 1) return -max / 2 * ((float)Math.Sqrt(1 - t * t) - 1) + min;

            t = t - 2;
            return max / 2 * ((float)Math.Sqrt(1 - t * t) + 1) + min;
        }

        public static Vector2 ElasticIn(float t, float totaltime, Vector2 min, Vector2 max)
        {
            // 無理でした
            return max;
        }

        public static Vector2 ElasticOut(float t, float totaltime, Vector2 min, Vector2 max)
        {
            // 無理でした
            return max;
        }

        public static Vector2 ElasticInOut(float t, float totaltime, Vector2 min, Vector2 max)
        {
            // 無理でした
            return max;
        }

        public static Vector2 BackIn(float t, float totaltime, Vector2 min, Vector2 max, float s)
        {
            max -= min;
            t /= totaltime;
            return max * t * t * ((s + 1) * t - s) + min;
        }

        public static Vector2 BackOut(float t, float totaltime, Vector2 min, Vector2 max, float s)
        {
            max -= min;
            t = t / totaltime - 1;
            return max * (t * t * ((s + 1) * t + s) + 1) + min;
        }

        public static Vector2 BackInOut(float t, float totaltime, Vector2 min, Vector2 max, float s)
        {
            max -= min;
            s *= 1.525f;
            t /= totaltime / 2;
            if (t < 1) return max / 2 * (t * t * ((s + 1) * t - s)) + min;

            t = t - 2;
            return max / 2 * (t * t * ((s + 1) * t + s) + 2) + min;
        }

        public static Vector2 BounceIn(float t, float totaltime, Vector2 min, Vector2 max)
        {
            max -= min;
            return max - BounceOut(totaltime - t, totaltime, new Vector2(), max) + min;
        }

        public static Vector2 BounceOut(float t, float totaltime, Vector2 min, Vector2 max)
        {
            max -= min;
            t /= totaltime;

            if (t < 1.0f / 2.75f)
            {
                return max * (7.5625f * t * t) + min;
            }
            else if (t < 2.0f / 2.75f)
            {
                t -= 1.5f / 2.75f;
                return max * (7.5625f * t * t + 0.75f) + min;
            }
            else if (t < 2.5f / 2.75f)
            {
                t -= 2.25f / 2.75f;
                return max * (7.5625f * t * t + 0.9375f) + min;
            }
            else
            {
                t -= 2.625f / 2.75f;
                return max * (7.5625f * t * t + 0.984375f) + min;
            }
        }

        public static Vector2 BounceInOut(float t, float totaltime, Vector2 min, Vector2 max)
        {
            if (t < totaltime / 2)
            {
                return BounceIn(t * 2, totaltime, new Vector2(), max - min) * 0.5f + min;
            }
            else
            {
                return BounceOut(t * 2 - totaltime, totaltime, new Vector2(), max - min) * 0.5f + min + (max - min) * 0.5f;
            }
        }

        public static Vector2 Linear(float t, float totaltime, Vector2 min, Vector2 max)
        {
            return (max - min) * t / totaltime + min;
        }

    }
}
