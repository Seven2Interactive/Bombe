//
// Port of Flambe classes.
// Flambe - Rapid game development
// https://github.com/aduros/flambe/blob/master/LICENSE.txt
using UnityEngine;

namespace Bombe
{
    /// <summary>
    /// An action that tweens an AnimatedFloat to a certain value.
    /// </summary>
	public class AnimateTo : IAction
	{

		private Tween _tween;
		private AnimatedFloat _value;
		private float _to;
		private float _seconds;
		private EaseFunction _easing;

		public AnimateTo(AnimatedFloat value, float to, float seconds, EaseFunction easing = null)
		{
			_value = value;
			_to = to;
			_seconds = seconds;
			_easing = easing;
		}
		
		public float Update(float dt, GameObject actor)
		{
			if (_tween == null)
			{
				_tween = new Tween(_value._, _to, _seconds, _easing);
				_value.behavior = _tween;
				_value.Update(dt); // Fake an update to account for this frame
			}
			if (_value.behavior != _tween)
			{
				var overtime = _tween.elapsed - _seconds;
				_tween = null;
				return (overtime > 0) ? Mathf.Max(0, dt - overtime) : 0;
			}
			return -1;
		}
		
	}
}

