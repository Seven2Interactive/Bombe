// Port of Flambe classes.
// Flambe - Rapid game development
// https://github.com/aduros/flambe/blob/master/LICENSE.txt

using UnityEngine;

namespace Bombe {

	/// Moves an object to a position.
	public class MoveToScreenPos : IAction {


		/* ---------------------------------------------------------------------------------------- */
		      
		public MoveToScreenPos (float x, float y, float seconds, EaseFunction ease = null)
		{
			Setup (x, y, seconds, ease);
		}

		/* ---------------------------------------------------------------------------------------- */

		public MoveToScreenPos(Transform transform, float x, float y, float seconds, EaseFunction ease = null)
		{
			_transform = transform;
			Setup (x, y, seconds, ease);
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
		void Setup(float x, float y, float seconds, EaseFunction ease = null) {
			_x = x;
			_y = y;
			_seconds = seconds;
			_ease = ease;
		}
		
		/* ---------------------------------------------------------------------------------------- */
		      
		public float Update (float dt, GameObject actor)
		{
			if (_tweenPercent == null) {

				_startPosition = _transform.position;

				if (_transform == null) {
					_transform = actor.GetComponent<Transform>();
				}

//				float fromX = _transform.position.x;
//				float fromY = _transform.position.y;
				_tweenPercent = new Tween(0, 1, _seconds, _ease);
			}

//			float percent = _tweenPercent.update(dt);
			Vector3 targetPos = Camera.main.ScreenToWorldPoint(new Vector3(_x, _y, 10f));
			Vector3 lerped = Vector3.Lerp(_startPosition, targetPos, _tweenPercent.Update(dt));
			_transform.position = new Vector3(lerped.x, lerped.y, _transform.position.z);


			if (_tweenPercent.IsComplete()) {
				float overtime = _tweenPercent.elapsed - _seconds;
				_tweenPercent = null;
				return (overtime > 0) ? Mathf.Max(0, dt - overtime) : 0;
			}

			return -1;
		}
		
		private Tween _tweenPercent;
		private Vector3 _startPosition;
		private Transform _transform;

		private float _x;
		private float _y;
		private float _seconds;
		private EaseFunction _ease;

	}
}
