using UnityEngine;

namespace Bombe {

	/// Moves an object to a position.
	public class ScaleTo : IAction {

		private Transform _transform;

		/* ---------------------------------------------------------------------------------------- */
		      
		public ScaleTo (float scaleX, float scaleY, float seconds, EaseFunction easingX = null, EaseFunction easingY = null)
		{
			Setup (scaleX, scaleY, seconds, easingX, easingY);
		}

		/* ---------------------------------------------------------------------------------------- */

		public ScaleTo(Transform transform, float scaleX, float scaleY, float seconds, EaseFunction easingX = null, EaseFunction easingY = null)
		{
			_transform = transform;
			Setup (scaleX, scaleY, seconds, easingX, easingY);
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
			_scaleX = x;
			_scaleY = y;
			_seconds = seconds;
			_easingX = easingX;
			_easingY = easingY;
		}
		
		/* ---------------------------------------------------------------------------------------- */
		      
		public float Update (float dt, GameObject actor)
		{
			if (_tweenX == null) {
				if (_transform == null) {
					_transform = actor.GetComponent<Transform>();
				}

				float fromX = _transform.localScale.x;
				float fromY = _transform.localScale.y;
				_tweenX = new Tween(fromX, _scaleX, _seconds, _easingX);
				_tweenY = new Tween(fromY, _scaleY, _seconds, (_easingY != null) ? _easingY : _easingX);
			}

			float toX = _tweenX.Update(dt);
			float toY = _tweenY.Update(dt);
			_transform.localScale = new Vector3(toX, toY, _transform.localScale.z);

			if (_tweenX.IsComplete() && _tweenY.IsComplete()) {
				float overtime = Mathf.Max(_tweenX.elapsed, _tweenY.elapsed) - _seconds;
				_tweenX = null;
				_tweenY = null;
				return (overtime > 0) ? Mathf.Max(0, dt - overtime) : 0;
			}

			return -1;
		}
		
		private Tween _tweenX;
		private Tween _tweenY;
		
		private float _scaleX;
		private float _scaleY;
		private float _seconds;
		private EaseFunction _easingX;
		private EaseFunction _easingY;

	}
}
