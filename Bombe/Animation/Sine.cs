//
// Flambe - Rapid game development
// https://github.com/aduros/flambe/blob/master/LICENSE.txt
using UnityEngine;

namespace Bombe
{
    /// <summary>
    /// Controls an AnimatedFloat using a Sine wave, typically endlessly.
    /// Useful for flashing a notification, or creating a throbbing animation effect without using a Script.
    /// </summary>
    /// <author>Kipp Ashford</author>
	public class Sine : Behavior
	{
        /// <summary>
        /// The end value.
        /// </summary>
		public float end;
        /// <summary>
        /// The starting value.
        /// </summary>
		public float start;
        /// <summary>
        /// The number of times to animate between the starting value and the end value.
        /// </summary>
		public float cycles;
        /// <summary>
        /// The speed, in seconds, it takes to animate between the starting value and the ending value (or the other way around)
        /// </summary>
		public AnimatedFloat speed;


        /// <summary>
        /// Stores the half value of PI for quicker calculations.
        /// </summary>
		private static float HALF_PI = .5f * Mathf.PI;
        /// <summary>
        /// The number of times to animate.
        /// </summary>
		private float _count;
        /// <summary>
        /// The total distance between the start and end values
        /// </summary>
		private float _distance;
        /// <summary>
        /// The middle of the start and end position, stored for quicker math.
        /// </summary>
		private float _center;

		/* ---------------------------------------------------------------------------------------- */

        /// <param name="start">The starting value for the animated float.</param>
        /// <param name="end">The last value for the animated float.</param>
        /// <param name="speed">The speed (in seconds) it takes to go from the start value to the end value.</param>
        /// <param name="cycles">The number of animation cycles to go through. A value of 0 will cycle forever,</param>
        ///   whereas a value of 1 will go from the start position, to the end position, and back to start.
        /// <param name="offset">The number of seconds to offset the animation. Useful for offseting the animation for a series of sine behaviors.</param>
		public Sine(float start, float end, float speed = 1f, float cycles = 0f, float offset = 0f)
		{
			this.start = start;
			this.end = end;
			this.cycles = cycles;
			this.speed = new AnimatedFloat(speed);
			
			_count = HALF_PI + offset * (Mathf.PI / speed); // Start at the start value plus the seconds to offset.
			_distance = (start - end) * .5f;
			_center = end + _distance;
		}

		/* ---------------------------------------------------------------------------------------- */

		public float Update(float dt)
		{
			this.speed.Update(dt);
			_count += dt * (Mathf.PI / speed._);
			if (IsComplete())
			{
				return _center + Mathf.PI * _distance;
			}
			return _center + Mathf.Sin(_count) * _distance;
		}

		/* ---------------------------------------------------------------------------------------- */

		public bool IsComplete()
		{
			return cycles > 0 && ((_count - HALF_PI) / Mathf.PI) * .5 >= cycles;
		}

		/* ---------------------------------------------------------------------------------------- */
	}
}
