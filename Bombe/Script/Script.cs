// Port of Flambe classes
// Flambe - Rapid game development
// https://github.com/aduros/flambe/blob/master/LICENSE.txt
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Bombe
{
	/// <summary>
	/// Manages a set of actions that are updated over time. Scripts simplify writing composable
	/// animations.
	/// Is the `Script` class in Flambe.
	/// </summary>
	public class Script : MonoBehaviour
	{

		private List<Handle> _handles;

		/* ---------------------------------------------------------------------------------------- */

		public Script()
		{
			_handles = new List<Handle>();
		}

		/* ---------------------------------------------------------------------------------------- */

		/// <summary>
		/// Add an action to this Script.
		/// </summary>
		public Disposable Run(IAction action)
		{
			Handle handle = new Handle(action);
			_handles.Add(handle);

			return handle;
		}

		/* ---------------------------------------------------------------------------------------- */

		/// <summary>
		/// Remove all actions from this ActionChain.
		/// </summary>
		public void StopAll()
		{
			for (int i = 0; i < _handles.Count; i++)
				_handles[i].Dispose();
		}
		
		public void Update()
		{
			float dt = Time.deltaTime;
			int ii = 0;
			while (ii < _handles.Count)
			{
				Handle handle = _handles[ii];
				if (handle.removed || handle.action.Update(dt, gameObject) >= 0)
				{
					_handles.RemoveAt(ii);
				}
				else
				{
					++ii;
				}
			}
		}

		/* ---------------------------------------------------------------------------------------- */
		
		protected class Handle : Disposable
		{
			private bool _removed;

			public bool removed
			{
				get
				{
					return _removed;
				}
				private set
				{
					_removed = value;
				}
			}

			public IAction action;
			
			public Handle(IAction action)
			{
				removed = false;
				this.action = action;
			}
			
			public void Dispose()
			{
				removed = true;
				// Check if the action needs to be disposed. 
				if (action is Disposable)
				{
					(action as Disposable).Dispose();
				}
				action = null;
			}
		}

		/* ---------------------------------------------------------------------------------------- */
	}
}
	
