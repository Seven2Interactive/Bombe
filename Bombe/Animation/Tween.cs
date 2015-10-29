// Port of Flambe classes.
// Flambe - Rapid game development
// https://github.com/aduros/flambe/blob/master/LICENSE.txt


namespace Bombe
{
	public class Tween : Behavior
	{
		private float _from;
		private float _to;
		private float _duration;
		private EaseFunction _easing;

		private float _elapsed;
		public float elapsed
		{
			get
			{
				return _elapsed;
			}

			private set
			{
				_elapsed = value;
			}
		}
		
		public Tween(float from, float to, float seconds, EaseFunction easing = null)
		{
			_from = from;
			_to = to;
			_duration = seconds;
			elapsed = 0;
			_easing = (easing != null) ? easing : Ease.Linear;
		}
		
		public float Update(float dt)
		{
			elapsed += dt;
			
			if (elapsed >= _duration)
			{
				return _to;
			}
			else
			{
				return _from + (_to - _from) * _easing(elapsed / _duration);
			}
		}
		
		public bool IsComplete()
		{
			return elapsed >= _duration;
		}
		
	}
}
