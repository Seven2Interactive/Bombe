// Port of Flambe classes.
// Flambe - Rapid game development
// https://github.com/aduros/flambe/blob/master/LICENSE.txt

using UnityEngine;
using System;

namespace Bombe {

	/// Moves an object to a position.
	public class RotateTo : IAction {

		/* ---------------------------------------------------------------------------------------- */
		      
		public RotateTo (float angle, float seconds, EaseFunction easing = null)
		{
			Setup (angle, seconds, easing);
		}
		
		/* ---------------------------------------------------------------------------------------- */
		      
		public RotateTo (Rigidbody2D body, float angle, float seconds, EaseFunction easing = null)
		{
			_InitialRotation = () => { return body.rotation; };
			_ApplyRotation = (float d) => body.rotation = d;

			Setup (angle, seconds, easing);
		}
		
		/* ---------------------------------------------------------------------------------------- */

		public RotateTo(Transform transform, float angle, float seconds, EaseFunction easing = null)
		{
			SetupTransform(transform);
			Setup (angle, seconds, easing);
		}
		
		/* ---------------------------------------------------------------------------------------- */

		/// <summary>
		/// Sets up the transform update and initialization delegates.
		/// </summary>
		/// <param name="transform">Transform.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="useLocal">If set to <c>true</c> use local.</param>
		void SetupTransform(Transform transform)
		{
			_InitialRotation = () => {
				return transform.rotation.eulerAngles.z;
			};
			_ApplyRotation = (float degrees) => {
				transform.rotation = Quaternion.Euler(0, 0, degrees);
			};
		}      

		/* ---------------------------------------------------------------------------------------- */

		/// <summary>
		/// Common constructor logic.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="seconds">Seconds.</param>
		/// <param name="easingX">Easing x.</param>
		/// <param name="easingY">Easing y.</param>
		void Setup(float degrees, float seconds, EaseFunction easing = null) {
			_destination = degrees;
			_seconds = seconds;
			_easing = easing;
		}
		
		/* ---------------------------------------------------------------------------------------- */
		      
		public float Update (float dt, GameObject actor)
		{
			if (_tween == null) {

				if (_InitialRotation == null) {
					// If a transform hasn't been set, use the transform from the actor script.
					SetupTransform(actor.GetComponent<Transform>());
				}

				_tween = new Tween(_InitialRotation(), _destination, _seconds, _easing);
			}

			_ApplyRotation( _tween.Update(dt) ); // Applies the position.

			if (_tween.IsComplete()) {
				float overtime = _tween.elapsed - _seconds;
				_tween = null;
				return (overtime > 0) ? Math.Max(0, dt - overtime) : 0;
			}

			return -1;
		}
		
		/* ---------------------------------------------------------------------------------------- */


		/// The delegate used to apply the newly tweened rotation.
		private Action<float> _ApplyRotation;

		/// The delegate which grabs the object's current rotation.
		private Func<float> _InitialRotation;

		/// The x property to transform.
		private Tween _tween; 

		/// The position to move to on the X.
		private float _destination; 

		/// The number of seconds to tween.
		private float _seconds; 

		/// The number of seconds to tween.
		private EaseFunction _easing;

	}
}
