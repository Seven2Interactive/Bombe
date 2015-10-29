// Port of Flambe classes.
// Flambe - Rapid game development
// https://github.com/aduros/flambe/blob/master/LICENSE.txt
using UnityEngine;

namespace Bombe
{
    /// <summary>
    /// An action that simply waits for a certain amount of time to pass before finishing.
    /// </summary>
	public class Delay : IAction
	{
		private float _duration;
		private float _elapsed;

		public Delay(float seconds)
		{
			_duration = seconds;
			_elapsed = 0;
		}
		
		public float Update(float dt, GameObject actor)
		{
			_elapsed += dt;
			if(_elapsed >= _duration)
			{
				float overtime = _elapsed - _duration;
				_elapsed = 0;
				return dt - overtime;
			}
			return -1;
		}
		
	}
}
