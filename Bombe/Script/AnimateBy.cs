// Port of Flambe classes.
// Flambe - Rapid game development
// https://github.com/aduros/flambe/blob/master/LICENSE.txt
using UnityEngine;

namespace Bombe
{

    /// <summary>
    /// An action that tweens an AnimatedFloat by a certain delta.
    /// </summary>
	public class AnimateBy : IAction
	{
		private Tween _tween;
		private AnimatedFloat _value;
		private float _by;
		private float _seconds;
		private EaseFunction _easing;

		public AnimateBy(AnimatedFloat value, float by, float seconds, EaseFunction easing = null)
		{
			_value = value;
			_by = by;
			_seconds = seconds;
			_easing = easing;
		}
		
		public float Update(float dt, GameObject actor)
		{
			if (_tween == null)
			{
				_tween = new Tween(_value._, _value._ + _by, _seconds, _easing);
				_value.behavior = _tween;
				_value.Update(dt); // Fake an update to account for this frame
			}
			if (_value.behavior != _tween)
			{
				float overtime = _tween.elapsed - _seconds;
				_tween = null;
				return (overtime > 0) ? Mathf.Max(0, dt - overtime) : 0;
			}
			return -1;
		}
		
	}
}
