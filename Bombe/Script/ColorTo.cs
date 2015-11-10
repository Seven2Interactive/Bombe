//
// Port of Flambe classes.
// Flambe - Rapid game development
// https://github.com/aduros/flambe/blob/master/LICENSE.txt
using UnityEngine;
using System;

namespace Bombe
{
    /// <summary>
    /// An action that tweens from one color to another, using a passed in callback to apply the changes.
    /// </summary>
	public class ColorTo : IAction
	{
		private Tween _tween;
		private Color _from;
		private Color _to;
		private float _seconds;
		private EaseFunction _easing;
		private Action<Color> _updateDelegate;

		/* ---------------------------------------------------------------------------------------- */
		      
		public ColorTo(Color start, Color to, float seconds, Action<Color> updateDelegate, EaseFunction easing = null)
		{
			_from = start;
			_to = to;
			_seconds = seconds;
			_easing = easing;
			_updateDelegate = updateDelegate;
		}

		/* ---------------------------------------------------------------------------------------- */
		      
		public float Update(float dt, GameObject actor)
		{
			if (_tween == null)
			{
				_tween = new Tween(0, 1, _seconds, _easing);
			}

			_updateDelegate(Color.Lerp(_from, _to, _tween.Update(dt)));

			if (_tween.IsComplete())
			{
				var overtime = _tween.elapsed - _seconds;
				_tween = null;
				return (overtime > 0) ? Mathf.Max(0, dt - overtime) : 0;
			}
			return -1;
		}
		
	}
}

