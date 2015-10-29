using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Bombe
{
    /// <summary>
    /// Port of Flambe classes
    /// https://github.com/aduros/flambe/blob/master/LICENSE.txt
    /// </summary>
	public class Disposer : MonoBehaviour, Disposable
	{

		private List<Disposable> _disposables;

		/* ---------------------------------------------------------------------------------------- */

		public Disposer()
		{
			_disposables = new List<Disposable>();
		}

		/* ---------------------------------------------------------------------------------------- */

        /// <summary>
        /// Add a Disposable, so that it also gets disposed when this component does.
        /// </summary>
		public Disposer Add(Disposable disposable)
		{
			_disposables.Add(disposable);
			return this;
		}

		/* ---------------------------------------------------------------------------------------- */

        /// <summary>
        /// Remove a Disposable from this disposer.
        /// </summary>
		public bool Remove(Disposable disposable)
		{
			return _disposables.Remove(disposable);
		}

		/* ---------------------------------------------------------------------------------------- */

        /// <summary>
        /// Chainable convenience method for connecting a signal listener and adding its SignalConnection
        /// to this disposer.
        /// </summary>
		public Disposer Connect(Signal signal, Action listener)
		{
			Add(signal.Connect(listener));
			return this;
		}

		/* ---------------------------------------------------------------------------------------- */

        /// <summary>
        /// Chainable convenience method for connecting a signal listener and adding its SignalConnection
        /// to this disposer.
        /// </summary>
		public Disposer Connect<T>(Signal<T> signal, Action<T> listener)
		{
			Add(signal.Connect(listener));
			return this;
		}

		/* ---------------------------------------------------------------------------------------- */

        /// <summary>
        /// Chainable convenience method for connecting a signal listener and adding its SignalConnection
        /// to this disposer.
        /// </summary>
		public Disposer Connect<T, U>(Signal<T, U> signal, Action<T, U> listener)
		{
			Add(signal.Connect(listener));
			return this;
		}

		/* ---------------------------------------------------------------------------------------- */

		public void OnDestroy()
		{
			FreeDisposables();
		}

		/* ---------------------------------------------------------------------------------------- */

		public void Dispose()
		{
			FreeDisposables(); // Cleanup even if this component had no owner
		}

		/* ---------------------------------------------------------------------------------------- */

		private void FreeDisposables()
		{
			List<Disposable> snapshot = _disposables;
			_disposables = new List<Disposable>();
			foreach (Disposable disposable in snapshot)
			{
				disposable.Dispose();
			}
		}

		/* ---------------------------------------------------------------------------------------- */


	}

}

