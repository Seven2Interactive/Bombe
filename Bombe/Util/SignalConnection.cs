using UnityEngine;
using System.Collections;
using System;


namespace Bombe
{

	public interface SignalConnection : Disposable {
		SignalConnection Once();
	}

    /// <summary>
    /// Port of Flambe classes
    /// https://github.com/aduros/flambe/blob/master/LICENSE.txt
    /// </summary>
	public class SignalConnectionBase : SignalConnection
	{
        /// <summary>
        /// True if the listener will remain connected after being used.
        /// </summary>
		public bool stayInList;	

		public SignalConnectionBase _next = null;
		
		public Action _listener;

		protected Signal _signal;

		/* ---------------------------------------------------------------------------------------- */

		public SignalConnectionBase (Signal signal, Action listener)
		{
			_signal = signal;
			_listener = listener;
			stayInList = true;
		}

		/* ---------------------------------------------------------------------------------------- */

        /// <summary>
        /// Tells the connection to dispose itself after being used once.
        /// </summary>
		public SignalConnection Once ()
		{
			stayInList = false;
			return this;
		}

		/* ---------------------------------------------------------------------------------------- */	

        /// <summary>
        /// Disconnects the listener from the signal.
        /// </summary>
		public virtual void Dispose ()
		{
			if (_signal != null) 
			{
				_signal.Disconnect(this);
				_signal = null;
			}
		}

	}

	/* ---------------------------------------------------------------------------------------- */
	      
	public class SignalConnectionBase<T> : SignalConnection
	{
		/// <summary>
		/// True if the listener will remain connected after being used.
		/// </summary>
		public bool stayInList;	
		
		public SignalConnectionBase<T> _next = null;
		
		public Action<T> _listener;
		
		protected Signal<T> _signal;

		/* ---------------------------------------------------------------------------------------- */
		
		public SignalConnectionBase(Signal<T> signal, Action<T> listener)
		{
			_signal = signal;
			_listener = listener;
			stayInList = true;
		}

		/* ---------------------------------------------------------------------------------------- */
		
		/// <summary>
		/// Tells the connection to dispose itself after being used once.
		/// </summary>
		public SignalConnection Once()
		{
			stayInList = false;
			return this;
		}

		/* ---------------------------------------------------------------------------------------- */	
		
		/// <summary>
		/// Disconnects the listener from the signal.
		/// </summary>
		public void Dispose()
		{
			if (_signal != null) 
			{
				_signal.Disconnect(this);
				_signal = null;
			}
		}

	}
	
	/* ---------------------------------------------------------------------------------------- */


	public class SignalConnectionBase<T, U> : SignalConnection
	{
		/// <summary>
		/// True if the listener will remain connected after being used.
		/// </summary>
		public bool stayInList;	
		
		public SignalConnectionBase<T, U> _next = null;
		
		public Action<T, U> _listener;
		
		protected Signal<T, U> _signal;

		/* ---------------------------------------------------------------------------------------- */

		public SignalConnectionBase(Signal<T, U> signal, Action<T, U> listener)
		{
			_signal = signal;
			_listener = listener;
			stayInList = true;
		}

		/* ---------------------------------------------------------------------------------------- */
		
		/// <summary>
		/// Tells the connection to dispose itself after being used once.
		/// </summary>
		public SignalConnection Once()
		{
			stayInList = false;
			return this;
		}

		/* ---------------------------------------------------------------------------------------- */	
		
		/// <summary>
		/// Disconnects the listener from the signal.
		/// </summary>
		public void Dispose()
		{
			if (_signal != null) 
			{
				_signal.Disconnect(this);
				_signal = null;
			}
		}

	}

	      
}
