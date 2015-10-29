// Port of Flambe classes.
// Flambe - Rapid game development
// https://github.com/aduros/flambe/blob/master/LICENSE.txt
using UnityEngine;
using System.Collections.Generic;

namespace Bombe
{
    /// <summary>
    /// An action that manages a list of other actions, running them together in parallel until the
    /// first of them finishes.
    /// </summary>
	public class FirstOf : IAction
	{
		private List<IAction> _runningActions;

		public FirstOf(params IAction[] actions)
		{
			_runningActions = (actions != null && actions.Length > 0) ? new List<IAction>(actions) : new List<IAction>();
		}
		
		public void Add(IAction action)
		{
			_runningActions.Add(action);
		}
		
		public bool Remove(IAction action)
		{
			int idx = _runningActions.IndexOf(action);
			if (idx < 0)
			{
				return false;
			}
			_runningActions[idx] = null;
			return true;
		}
		
		public void RemoveAll()
		{
			_runningActions = new List<IAction>();
		}
		
		public float Update(float dt, GameObject actor)
		{
			foreach (IAction action in _runningActions)
			{
				if (action != null)
				{
					float spent = action.Update(dt, actor);
					if (spent >= 0)
					{
						return spent;
					}
				}
			}
			
			return -1;
		}

	}
}
