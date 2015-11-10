//
// Port of Flambe classes.
// Flambe - Rapid game development
// https://github.com/aduros/flambe/blob/master/LICENSE.txt
using UnityEngine;

namespace Bombe
{
    /// <summary>
    /// An action that tweens an object's alpha value
    /// </summary>
	public class AlphaTo : IAction
	{

		private Tween _tween;
		private Transform _transform;
		private Renderer _renderer;

		private CanvasGroup _canvasGroup;
		private float _to;
		private float _seconds;
		private EaseFunction _easing;
		
		/* ---------------------------------------------------------------------------------------- */
		      
		public AlphaTo(Transform transform, float to, float seconds, EaseFunction easing = null)
		{
			_transform = transform;
			_to = to;
			_seconds = seconds;
			_easing = easing;
		}
		
		/* ---------------------------------------------------------------------------------------- */
		      
		public AlphaTo(CanvasGroup canvasGroup, float to, float seconds, EaseFunction easing = null)
		{
			_canvasGroup = canvasGroup;
			_to = to;
			_seconds = seconds;
			_easing = easing;
		}
		
		/* ---------------------------------------------------------------------------------------- */
		      
		public float Update(float dt, GameObject actor)
		{
			if (_tween == null)
			{
				if (_canvasGroup == null) {
					_renderer = _transform.GetComponentInChildren<Renderer>();
				}
				float fromAlpha = _canvasGroup != null ? _canvasGroup.alpha : _renderer.material.color.a;
				_tween = new Tween(fromAlpha, _to, _seconds, _easing);
			}

			float percent = Mathf.Clamp(_tween.Update(dt), 0f, 1f);

			// Update the target color.
			if (_renderer != null) {
				_renderer.material.color = new Color(1, 1, 1, percent);
			} else {
				_canvasGroup.alpha = percent;
			}


			if (_tween.isComplete())
			{
				var overtime = _tween.elapsed - _seconds;
				_tween = null;
				return (overtime > 0) ? Mathf.Max(0, dt - overtime) : 0;
			}
			return -1;
		}
		
	}
}

