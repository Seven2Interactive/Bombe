// Port of Flambe classes.
// Flambe - Rapid game development
// https://github.com/aduros/flambe/blob/master/LICENSE.txt

using UnityEngine;
using System;

namespace Bombe {

	/// Moves an object to a position.
	public class MoveTo : IAction {

		/* ---------------------------------------------------------------------------------------- */
		      
		public MoveTo (float x, float y, float seconds, EaseFunction easingX = null, EaseFunction easingY = null, bool useLocal = false)
		{
			_useLocal = useLocal;
			Setup (x, y, seconds, easingX, easingY);
		}
		
		/* ---------------------------------------------------------------------------------------- */
		      
		public MoveTo (Rigidbody2D body, float x, float y, float seconds, EaseFunction easingX = null, EaseFunction easingY = null)
		{
			_initPosition = () => { return new Vector3(body.position.x, body.position.y, 0); };
			_applyPosition = (float pX, float pY, float _) => { body.MovePosition(new Vector2(pX, pY)); };

			Setup (x, y, seconds, easingX, easingY);
		}
		
		/* ---------------------------------------------------------------------------------------- */

		public MoveTo(Transform transform, float x, float y, float seconds, EaseFunction easingX = null, EaseFunction easingY = null, bool useLocal = false)
		{
			_useLocal = useLocal;

			SetupTransform(transform, useLocal);
			Setup (x, y, seconds, easingX, easingY);
		}
		
		/* ---------------------------------------------------------------------------------------- */

		/// <summary>
		/// Sets up the transform update and initialization delegates.
		/// </summary>
		/// <param name="transform">Transform.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="useLocal">If set to <c>true</c> use local.</param>
		void SetupTransform(Transform transform, bool useLocal) {

			if (!_useLocal) {
				_initPosition = () => { return transform.position; };
				_applyPosition = (float x, float y, float z) => { transform.position = new Vector3(x, y, transform.position.z); };
			} else {
				_initPosition = () => { return transform.localPosition; };
				_applyPosition = (float x, float y, float z) => { transform.localPosition = new Vector3(x, y, transform.localPosition.z); };
			}
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
		void Setup(float x, float y, float seconds, EaseFunction easingX = null, EaseFunction easingY = null) {
			_x = x;
			_y = y;
			_seconds = seconds;
			_easingX = easingX;
			_easingY = easingY;
		}
		
		/* ---------------------------------------------------------------------------------------- */
		      
		public float Update (float dt, GameObject actor)
		{
			if (_tweenX == null) {

				if (_initPosition == null) {
					SetupTransform(actor.GetComponent<Transform>(), _useLocal);
				}

				Vector3 from = _initPosition();

				_tweenX = new Tween(from.x, _x, _seconds, _easingX);
				_tweenY = new Tween(from.y, _y, _seconds, (_easingY != null) ? _easingY : _easingX);
			}

			float toX = _tweenX.Update(dt);
			float toY = _tweenY.Update(dt);

			_applyPosition(toX, toY, 0); // Applies the position.

			if (_tweenX.IsComplete() && _tweenY.IsComplete()) {
				float overtime = Mathf.Max(_tweenX.elapsed, _tweenY.elapsed) - _seconds;
				_tweenX = null;
				_tweenY = null;
				return (overtime > 0) ? Math.Max(0, dt - overtime) : 0;
			}

			return -1;
		}
		
		/* ---------------------------------------------------------------------------------------- */


		/// The delegate used to apply the newly tweened position.
		private Action<float, float, float> _applyPosition;

		/// The delegate which grabs the object's current position.
		private Func<Vector3> _initPosition;

		/// Used for transforms.
		private bool _useLocal; 

		/// The x property to transform.
		private Tween _tweenX; 

		/// The y property to transform.
		private Tween _tweenY; 

		/// The position to move to on the X.
		private float _x; 

		/// The position to move to on the Y.
		private float _y; 

		/// The number of seconds to tween.
		private float _seconds; 

		/// The number of seconds to tween.
		private EaseFunction _easingX;

		/// The number of seconds to tween.
		private EaseFunction _easingY;

	}
}
