//
// Flambe - Rapid game development
// https://github.com/aduros/flambe/blob/master/LICENSE.txt

namespace Bombe
{
	public class RepeatTween : Behavior
	{
		public float elapsed;
		
		private float _from;
		private float _to;
		private float _duration;
		private float _cycles;
		private float _count;
		private EaseFunction _easing;
		private float _originTo;
		private float _originFrom;
		private bool _bForward = true;
		private bool _bYoyo = false;
		
		public RepeatTween(float from, float to, float seconds, float cycles = 0, bool bYoyo = false, EaseFunction easing = null)
		{
			_from = from;
			_to = to;
			_duration = seconds;
			_count = 0;
			_cycles = cycles;
			_bYoyo = bYoyo;
			elapsed = 0;
			_easing = (easing != null) ? easing : Ease.Linear;
		}
		
		public float Update(float dt)
		{
			elapsed += dt;
			
			if (IsComplete()) 
			{
				return _to;
			}
			else 
			{
				if (elapsed >= _duration)
				{
					_count ++;
					elapsed = 0;
					// If we yoyo, the swap which direction to play in, if not yoyo, then only go forward. 
					_bForward = _bYoyo == true ? !_bForward : true;
				}
				
				if ( _bForward )
					return _from + (_to - _from) * _easing(elapsed / _duration);
				else
					return _from + (_to - _from) * _easing(1 - (elapsed / _duration));
			}
		}
		
		public bool IsComplete ()
		{
			return (elapsed >= _duration && _cycles > 0 && _count >= _cycles);
		}
		
	}
}
