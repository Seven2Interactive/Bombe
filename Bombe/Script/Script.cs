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
		/// The timescale to use to modify the delta time of actions.
		public AnimatedFloat timeScale = new AnimatedFloat(1f);

		/// If the script should automatically destroy itself when done running.
		public bool destroyOnComplete = false;

		/// Use the FixedUpdate loop in order to sync with physics simulations. Defaults to false.
		public bool useFixed = false;

		/// Use unscaled delta time in order to run at normal, unscaled time. Useful for pause menus.
		public bool useUnscaledTime = false;

		/// The list of actions to be played so they can be disposed of later.
		private List<Handle> _handles;

		/// <summary>
		/// Gets a value indicating whether this <see cref="Bombe.Script"/> is running.
		/// </summary>
		/// <value><c>true</c> if running; otherwise, <c>false</c>.</value>
		public bool running { get; private set; }

		/* ---------------------------------------------------------------------------------------- */

		public Script()
		{
			_handles = new List<Handle>();
		}
		
		/* ---------------------------------------------------------------------------------------- */
		
		public Script UseFixedUpdate(bool useFixed) {
			this.useFixed = useFixed;
			return this;
		}
		
		/* ---------------------------------------------------------------------------------------- */
		      
		public Script UseUnscaledTime(bool useUnscaledTime) {
			this.useUnscaledTime = useUnscaledTime;
			return this;
		}

		/* ---------------------------------------------------------------------------------------- */
		
		public Script DestroyOnComplete(bool destroyOnComplete) {
			this.destroyOnComplete = destroyOnComplete;
			return this;
		}

		/* ---------------------------------------------------------------------------------------- */

		/// <summary>
		/// Add an action to this Script.
		/// </summary>
		public Disposable Run(IAction action)
		{
			Handle handle = new Handle(action);
			_handles.Add(handle);
			running = true;
			return handle;
		}

		/* ---------------------------------------------------------------------------------------- */

		/// <summary>
		/// Remove all actions from this Script.
		/// </summary>
		public void StopAll()
		{
			int ii = _handles.Count;
			while (ii-->0) {
				_handles[ii].Dispose();
			}
			_handles.Clear();
			bool wasRunning = running;
			running = false;

			if (wasRunning && destroyOnComplete) {
				Destroy (this);
			}

		}
		
		/* ---------------------------------------------------------------------------------------- */

		private void UpdateLoop(float dt) {
			timeScale.Update(dt);
			float ts = timeScale._;
			int ii = _handles.Count;
			
			while (ii-->0) {
				Handle handle = _handles[ii];
				if (handle.removed || handle.action.Update(dt * ts, gameObject) >= 0)
				{
					_handles.RemoveAt(ii);
					handle.Dispose();
				}
			}

			if (_handles.Count == 0) {
				running = false;

				if (destroyOnComplete) {
					Destroy(this);
				}
			}

		} 

		
		/* ---------------------------------------------------------------------------------------- */

		void OnDestroy() {
			if (running) {
				StopAll();
			}
		}      

		/* ---------------------------------------------------------------------------------------- */
		            
		void Update()
		{
			if (!useFixed) {
				if (!useUnscaledTime && Time.timeScale > 0) {
					UpdateLoop(Time.deltaTime);	
				} else {
					UpdateLoop(Time.unscaledDeltaTime);
				}
			}
		}
		
		/* ---------------------------------------------------------------------------------------- */

		void FixedUpdate() {
			if (useFixed) {
				if (!useUnscaledTime && Time.timeScale > 0) {
					UpdateLoop(Time.deltaTime);	
				} else {
					UpdateLoop(Time.unscaledDeltaTime);
				}
			}
		}		      

		/* ---------------------------------------------------------------------------------------- */
		
		protected class Handle : Disposable
		{
			public bool removed { get; private set; }
			public IAction action;
			
			/* ---------------------------------------------------------------------------------------- */
			      
			public Handle(IAction action)
			{
				removed = false;
				this.action = action;
			}
			
			/* ---------------------------------------------------------------------------------------- */
			      
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
	
