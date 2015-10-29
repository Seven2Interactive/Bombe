// Port of Flambe classes.
// Flambe - Rapid game development
// https://github.com/aduros/flambe/blob/master/LICENSE.txt

//using Lambda;
//using flambe.util.Arrays;
using UnityEngine;
using System.Collections.Generic;

namespace Bombe
{

    /// <summary>
    /// An action that manages a list of other actions, running them one-by-one sequentially until they
    /// all finish.
    /// </summary>
	public class Sequence : IAction, Disposable
	{
		private List<IAction> _runningActions;
		private int _idx;

		public Sequence(params IAction[] actions)
		{
			_idx = 0;

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
			_idx = 0;
			_runningActions = new List<IAction>();
		}
		
		public float Update(float dt, GameObject actor)
		{
			// The total time taken by the actions updated this frame
			float total = 0.0f;

			while (true)
			{
				IAction action = _runningActions[_idx];
				if (action != null)
				{
					float spent = action.Update(dt - total, actor);
					if (spent >= 0)
					{
						// This action completed, add it to the total time
						total += spent;
					}
					else
					{
						// This action didn't complete, so neither will this sequence
						return -1;
					}
				}
				
				++_idx;
				if (_idx >= _runningActions.Count)
				{
					// If this is the last action, reset to the starting position and finish
					_idx = 0;
					break;
				}
				else if (total > dt)
				{
					// Otherwise, if there are still actions but not enough time to complete them
					return -1;
				}
			}
			
			return total;
		}

		public void Dispose()
		{
			foreach (IAction action in _runningActions)
			{
				if ( action is Disposable )
				{
					(action as Disposable).Dispose();
				}
			}
		}
		
	}
}
