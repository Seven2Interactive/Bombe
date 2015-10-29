// Port of Flambe classes.
// Flambe - Rapid game development
// https://github.com/aduros/flambe/blob/master/LICENSE.txt
using UnityEngine;
using System;

namespace Bombe
{
    /// <summary>
    /// An action that calls a given function once and immediately completes.
    /// </summary>
	public class CallFunction : IAction
	{
		private Action _fn;

        /// <param name="fn">The function to call when this action is run.</param>
		public CallFunction(Action fn)
		{
			_fn = fn;
		}
		
		public float Update(float dt, GameObject actor)
		{
			_fn();
			return 0;
		}
		
	}
}
