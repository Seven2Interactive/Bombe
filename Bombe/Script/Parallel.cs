// Port of Flambe classes.
// Flambe - Rapid game development
// https://github.com/aduros/flambe/blob/master/LICENSE.txt
using UnityEngine;
using System.Collections.Generic;

namespace Bombe
{
    /// <summary>
    /// An action that manages a list of other actions, running them together in parallel until they all
    /// finish.
    /// </summary>
	public class Parallel : IAction, Disposable
	{

		private List<IAction> _runningActions;
		private List<IAction> _completedActions;

		public Parallel(params IAction[] actions)
		{
			_completedActions = new List<IAction>();
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
			_completedActions = new List<IAction>();
		}
		
		public float Update(float dt, GameObject actor)
		{
			bool done = true;
			float maxSpent = 0.0f;

			for (int ii = 0; ii < _runningActions.Count; ii++)
			{
				IAction action = _runningActions[ii];
				if (action != null)
				{
					float spent = action.Update(dt, actor);
					if (spent >= 0)
					{
						_runningActions[ii] = null;
						_completedActions.Add(action);
						if (spent > maxSpent)
						{
							maxSpent = spent;
						}
					}
					else
					{
						// We can't possibly finish this frame, but continue ticking the rest of the
						// actions anyways
						done = false;
					}
				}
			}
			
			if (done)
			{
				_runningActions = _completedActions;
				_completedActions = new List<IAction>();
				return maxSpent;
			}
			return -1;
		}

		public void Dispose()
		{
			foreach ( IAction action in _runningActions)
			{
				if ( action is Disposable )
				{
					(action as Disposable).Dispose();
				}
			}
		}
	}
}
