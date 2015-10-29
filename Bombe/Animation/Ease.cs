// Port of Flambe classes.
// Flambe - Rapid game development
// https://github.com/aduros/flambe/blob/master/LICENSE.txt
using UnityEngine;

namespace Bombe
{

    /// <summary>
    /// Receives and returns a number between [0,1].
    /// </summary>
	public delegate float EaseFunction(float nValue);


    /// <summary>
    /// Easing functions that can be used to animate values. For a cheat sheet, see <http://easings.net>.
    /// </summary>
	public class Ease
	{
		// Adapted from FlashPunk:
		// https://github.com/Draknek/FlashPunk/blob/master/net/flashpunk/utils/Ease.as
		//
		// Operation of in/out easers:
		//
		// in(t)
		//        return t;
		// out(t)
		//         return 1 - in(1 - t);
		// inOut(t)
		//         return (t <= .5) ? in(t * 2) / 2 : out(t * 2 - 1) / 2 + .5;
		
		private static float PIhalf = Mathf.PI / 2;
		private static float PI = Mathf.PI;
		private static float PI2 = Mathf.PI * 2;
		private static float B1 = 1f / 2.75f;
		private static float B2 = 2f / 2.75f;
		private static float B3 = 1.5f / 2.75f;
		private static float B4 = 2.5f / 2.75f;
		private static float B5 = 2.25f / 2.75f;
		private static float B6 = 2.625f / 2.75f;
		private static float ELASTIC_AMPLITUDE = 1f;
		private static float ELASTIC_PERIOD = 0.4f;



        /// <summary>
        /// Linear, no easing.
        /// </summary>
		public static float Linear(float t)
		{
			return t;
		}
		
        /// <summary>
        /// Quadratic in.
        /// </summary>
		public static float QuadIn(float t)
		{
			return t * t;
		}
		
        /// <summary>
        /// Quadratic out.
        /// </summary>
		public static float QuadOut(float t)
		{
			return t * (2f - t);
		}
		
        /// <summary>
        /// Quadratic in and out.
        /// </summary>
		public static float QuadInOut(float t)
		{
			return t <= .5f ? t * t * 2f : 1f - (--t) * t * 2f;
		}
		
        /// <summary>
        /// Cubic in.
        /// </summary>
		public static float CubeIn(float t)
		{
			return t * t * t;
		}
		
        /// <summary>
        /// Cubic out.
        /// </summary>
		public static float CubeOut(float t)
		{
			return 1f + (--t) * t * t;
		}
		
        /// <summary>
        /// Cubic in and out.
        /// </summary>
		public static float CubeInOut(float t)
		{
			return t <= .5f ? t * t * t * 4f : 1f + (--t) * t * t * 4f;
		}
		
        /// <summary>
        /// Quartic in.
        /// </summary>
		public static float QuartIn(float t)
		{
			return t * t * t * t;
		}
		
        /// <summary>
        /// Quartic out.
        /// </summary>
		public static float QuartOut(float t)
		{
			return 1f - (--t) * t * t * t;
		}
		
        /// <summary>
        /// Quartic in and out.
        /// </summary>
		public static float QuartInOut(float t)
		{
			return t <= .5f ? t * t * t * t * 8f : (1f - (t = t * 2f - 2f) * t * t * t) / 2f + .5f;
		}
		
        /// <summary>
        /// Quintic in.
        /// </summary>
		public static float QuintIn(float t)
		{
			return t * t * t * t * t;
		}
		
        /// <summary>
        /// Quintic out.
        /// </summary>
		public static float QuintOut(float t)
		{
			return (t = t - 1f) * t * t * t * t + 1f;
		}
		
        /// <summary>
        /// Quintic in and out.
        /// </summary>
		public static float QuintInOut(float t)
		{
			return ((t *= 2f) < 1f) ? (t * t * t * t * t) / 2f : ((t -= 2f) * t * t * t * t + 2f) / 2f;
		}
		
        /// <summary>
        /// Sine in.
        /// </summary>
		public static float SineIn(float t)
		{
			return 1f - Mathf.Cos(PIhalf * t);
		}
		
        /// <summary>
        /// Sine out.
        /// </summary>
		public static float SineOut(float t)
		{
			return Mathf.Sin(PIhalf * t);
		}
		
        /// <summary>
        /// Sine in and out.
        /// </summary>
		public static float SineInOut(float t)
		{
			return .5f - Mathf.Cos(PI * t) / 2f;
		}
		
        /// <summary>
        /// Bounce in.
        /// </summary>
		public static float BounceIn(float t)
		{
			t = 1f - t;
			if (t < B1)
			{
				return 1f - 7.5625f * t * t;
			}
			if (t < B2)
			{
				return 1f - (7.5625f * (t - B3) * (t - B3) + .75f);
			}
			if (t < B4)
			{
				return 1f - (7.5625f * (t - B5) * (t - B5) + .9375f);
			}
			return 1f - (7.5625f * (t - B6) * (t - B6) + .984375f);
		}
		
        /// <summary>
        /// Bounce out.
        /// </summary>
		public static float BounceOut(float t)
		{
			if (t < B1)
			{
				return 7.5625f * t * t;
			}
			if (t < B2)
			{
				return 7.5625f * (t - B3) * (t - B3) + .75f;
			}
			if (t < B4)
			{
				return 7.5625f * (t - B5) * (t - B5) + .9375f;
			}
			return 7.5625f * (t - B6) * (t - B6) + .984375f;
		}
		
        /// <summary>
        /// Bounce in and out.
        /// </summary>
		public static float BounceInOut(float t)
		{
			if (t < .5f)
			{
				t = 1f - t * 2f;
				if (t < B1)
				{
					return (1f - 7.5625f * t * t) / 2f;
				}
				if (t < B2)
				{
					return (1f - (7.5625f * (t - B3) * (t - B3) + .75f)) / 2f;
				}
				if (t < B4)
				{
					return (1f - (7.5625f * (t - B5) * (t - B5) + .9375f)) / 2f;
				}
				return (1f - (7.5625f * (t - B6) * (t - B6) + .984375f)) / 2f;
			}
			t = t * 2f - 1f;
			if (t < B1)
			{
				return (7.5625f * t * t) / 2f + .5f;
			}
			if (t < B2)
			{
				return (7.5625f * (t - B3) * (t - B3) + .75f) / 2f + .5f;
			}
			if (t < B4)
			{
				return (7.5625f * (t - B5) * (t - B5) + .9375f) / 2f + .5f;
			}
			return (7.5625f * (t - B6) * (t - B6) + .984375f) / 2f + .5f;
		}
		
        /// <summary>
        /// Circle in.
        /// </summary>
		public static float CircIn(float t)
		{
			return 1f - Mathf.Sqrt(1f - t * t);
		}
		
        /// <summary>
        /// Circle out.
        /// </summary>
		public static float CircOut(float t)
		{
			--t;
			return Mathf.Sqrt(1f - t * t);
		}
		
        /// <summary>
        /// Circle in and out.
        /// </summary>
		public static float CircInOut(float t)
		{
			return t <= .5f ? (Mathf.Sqrt(1f - t * t * 4f) - 1f) / -2f : (Mathf.Sqrt(1f - (t * 2f - 2f) * (t * 2f - 2f)) + 1f) / 2f;
		}
		
        /// <summary>
        /// Exponential in.
        /// </summary>
		public static float ExpoIn(float t)
		{
			return Mathf.Pow(2, 10 * (t - 1));
		}
		
        /// <summary>
        /// Exponential out.
        /// </summary>
		public static float ExpoOut(float t)
		{
			return -Mathf.Pow(2, -10 * t) + 1;
		}
		
        /// <summary>
        /// Exponential in and out.
        /// </summary>
		public static float ExpoInOut(float t)
		{
			return t < .5 ? Mathf.Pow(2, 10 * (t * 2 - 1)) / 2 : (-Mathf.Pow(2, -10 * (t * 2 - 1)) + 2) / 2;
		}
		
        /// <summary>
        /// Back in.
        /// </summary>
		public static float BackIn(float t)
		{
			return t * t * (2.70158f * t - 1.70158f);
		}
		
        /// <summary>
        /// Back out.
        /// </summary>
		public static float BackOut(float t)
		{
			return 1f - (--t) * (t) * (-2.70158f * t - 1.70158f);
		}
		
        /// <summary>
        /// Back in and out.
        /// </summary>
		public static float BackInOut(float t)
		{
			t *= 2;
			if (t < 1)
			{
				return t * t * (2.70158f * t - 1.70158f) / 2;
			}
			t -= 2;
			return (1 - t * t * (-2.70158f * t - 1.70158f)) / 2 + .5f;
		}
		
        /// <summary>
        /// Elastic in.
        /// </summary>
		public static float ElasticIn(float t)
		{
			return -(ELASTIC_AMPLITUDE * Mathf.Pow(2, 10 * (t -= 1)) * Mathf.Sin((t - (ELASTIC_PERIOD / PI2 * Mathf.Asin(1 / ELASTIC_AMPLITUDE))) * PI2 / ELASTIC_PERIOD));
		}
		
        /// <summary>
        /// Elastic out.
        /// </summary>
		public static float ElasticOut(float t)
		{
			return (ELASTIC_AMPLITUDE * Mathf.Pow(2, -10 * t) * Mathf.Sin((t - (ELASTIC_PERIOD / PI2 * Mathf.Asin(1 / ELASTIC_AMPLITUDE))) * PI2 / ELASTIC_PERIOD) + 1);
		}
		
        /// <summary>
        /// Elastic in and out.
        /// </summary>
		public static float ElasticInOut(float t)
		{
			if (t < 0.5f)
			{
				return -0.5f * (Mathf.Pow(2, 10 * (t -= 0.5f)) * Mathf.Sin((t - (ELASTIC_PERIOD / 4)) * PI2 / ELASTIC_PERIOD));
			}
			return Mathf.Pow(2, -10 * (t -= 0.5f)) * Mathf.Sin((t - (ELASTIC_PERIOD / 4)) * PI2 / ELASTIC_PERIOD) * 0.5f + 1;
		}
		
	}
}
