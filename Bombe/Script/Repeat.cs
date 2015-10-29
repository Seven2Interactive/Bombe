// Port of Flambe classes.
// Flambe - Rapid game development
// https://github.com/aduros/flambe/blob/master/LICENSE.txt
using UnityEngine;

namespace Bombe
{
    /// <summary>
    /// An action that repeats another action until it finishes a certain number of times.
    /// </summary>
	public class Repeat : IAction, Disposable
	{
		private IAction _action;
		private int _count;
		private int _remaining;


        /// <param name="count">The number of times to repeat the action, or -1 to repeat forever.</param>
		public Repeat(IAction action, int count = -1)
		{
			_action = action;
			_count = count;
			_remaining = count;
		}
		
		public float Update(float dt, GameObject actor)
		{
			if (_count == 0)
			{
				// Handle the special case of a 0-count Repeat
				return 0;
			}
			
			float spent = _action.Update(dt, actor);
			if (_count > 0 && spent >= 0 && --_remaining == 0)
			{
				_remaining = _count; // Reset state in case this Action is reused
				return spent;
			}
			
			// Keep repeating
			return -1;
		}

		public void Dispose()
		{
			if ( _action is Disposable )
			{
				(_action as Disposable).Dispose();
			}
		}
		
	}
}

