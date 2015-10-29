using UnityEngine;
using System.Collections;
using System;

namespace Bombe
{

    /// <summary>
    /// Port of Flambe classes
    /// https://github.com/aduros/flambe/blob/master/LICENSE.txt
    /// </summary>
	public class Signal
	{
		private static SignalConnectionBase DISPATCHING_SENTINEL = new SignalConnectionBase(null, null);
		private SignalConnectionBase _head;
		private Task _deferredTasks;

		/* ---------------------------------------------------------------------------------------- */

		public Signal(Action listener = null)
		{
			_head = (listener != null) ? new SignalConnectionBase(this, listener) : null;
			_deferredTasks = null;
		}

		/* ---------------------------------------------------------------------------------------- */
		
		/// <summary>
		/// Connects a listener to this signal.
		/// </summary>
		/// <param name="prioritize">True if this listener should fire before others.</param>
		public SignalConnection Connect(Action listener, bool prioritize = false)
		{
			return ConnectImpl(listener, prioritize);
		}
		
		/* ---------------------------------------------------------------------------------------- */
		
		/// <summary>
		/// Emit the signal, notifying each connected listener.
		/// </summary>
		public void Emit()
		{
			if(Dispatching())
			{
				Defer(delegate() {
					EmitImpl();
				});
			}
			else
			{
				EmitImpl();
			}
		}
		
		/* ---------------------------------------------------------------------------------------- */
		
		private void EmitImpl()
		{
			SignalConnectionBase head = WillEmit();
			SignalConnectionBase p = head;
			while(p != null)
			{
				p._listener();
				if(!p.stayInList)
				{
					p.Dispose();
				}
				p = p._next;
			}
			DidEmit(head);
		}

		/* ---------------------------------------------------------------------------------------- */

        /// <summary>
        /// Whether this signal has at least one listener.
        /// </summary>
		public bool HasListeners()
		{
			return _head != null;
		}

		/* ---------------------------------------------------------------------------------------- */

		protected SignalConnection ConnectImpl(Action listener, bool prioritize)
		{
			SignalConnectionBase conn = new SignalConnectionBase(this, listener);
			if(Dispatching())
			{
				Defer(delegate() {
					ListAdd(conn, prioritize);
				});
			}
			else
			{
				ListAdd(conn, prioritize);
			}
			return conn;
		}

		/* ---------------------------------------------------------------------------------------- */

		public void Disconnect(SignalConnectionBase conn)
		{
			if(Dispatching())
			{
				Defer(delegate() {
					ListRemove(conn);
				});
			}
			else
			{
				ListRemove(conn);
			}
		}

		/* ---------------------------------------------------------------------------------------- */

		protected void Defer(Action fn)
		{
			Task tail = null, p = _deferredTasks;
			while(p != null)
			{
				tail = p;
				p = p.next;
			}
			
			Task task = new Task(fn);
			if(tail != null)
			{
				tail.next = task;
			}
			else
			{
				_deferredTasks = task;
			}
		}

		/* ---------------------------------------------------------------------------------------- */

		protected SignalConnectionBase WillEmit()
		{
			// Should never happen, since the public emit methods will defer, but just in case...
			if(Dispatching())
			{
				throw new Exception();
			}
			
			SignalConnectionBase snapshot = _head;
			_head = DISPATCHING_SENTINEL;
			return snapshot;
		}

		/* ---------------------------------------------------------------------------------------- */

		protected void DidEmit(SignalConnectionBase head)
		{
			_head = head;
			
			Task snapshot = _deferredTasks;
			_deferredTasks = null;
			while(snapshot != null)
			{
				snapshot.fn();
				snapshot = snapshot.next;
			}
		}

		/* ---------------------------------------------------------------------------------------- */

		protected void ListAdd(SignalConnectionBase conn, bool prioritize)
		{
			if(prioritize)
			{
				// Prepend it to the beginning of the list
				conn._next = _head;
				_head = conn;
			}
			else
			{
				// Append it to the end of the list
				SignalConnectionBase tail = null, p = _head;
				while(p != null)
				{
					tail = p;
					p = p._next;
				}
				if(tail != null)
				{
					tail._next = conn;
				}
				else
				{
					_head = conn;
				}
			}
		}

		/* ---------------------------------------------------------------------------------------- */

		protected void ListRemove(SignalConnectionBase conn)
		{
			SignalConnectionBase prev = null, p = _head;
			while(p != null)
			{
				if(p == conn)
				{
					// Splice out p
					var next = p._next;
					if(prev == null)
					{
						_head = next;
					}
					else
					{
						prev._next = next;
					}
					return;
				}
				prev = p;
				p = p._next;
			}
		}

		/* ---------------------------------------------------------------------------------------- */

		protected bool Dispatching()
		{
			return _head == DISPATCHING_SENTINEL;
		}

			


	}

	////////////////
	/// ///////////
	/// ///////////
	/// 

	/// <summary>
	/// Port of Flambe classes
	/// https://github.com/aduros/flambe/blob/master/LICENSE.txt
	/// </summary>
	public class Signal<T>
	{
		private static SignalConnectionBase<T> DISPATCHING_SENTINEL = new SignalConnectionBase<T>(null, null);
		private SignalConnectionBase<T> _head;
		private Task _deferredTasks;
		
		/* ---------------------------------------------------------------------------------------- */
		
		public Signal(Action<T> listener = null)
		{
			_head = (listener != null) ? new SignalConnectionBase<T>(this, listener) : null;
			_deferredTasks = null;
		}
		
		/* ---------------------------------------------------------------------------------------- */
		
		/// <summary>
		/// Connects a listener to this signal.
		/// </summary>
		/// <param name="prioritize">True if this listener should fire before others.</param>
		public SignalConnection Connect(Action<T> listener, bool prioritize = false)
		{
			return ConnectImpl(listener, prioritize);
		}
		
		/* ---------------------------------------------------------------------------------------- */
		
		/// <summary>
		/// Emit the signal, notifying each connected listener.
		/// </summary>
		public void Emit(T arg1)
		{
			if(Dispatching())
			{
				Defer(delegate() {
					EmitImpl(arg1);
				});
			}
			else
			{
				EmitImpl(arg1);
			}
		}
		
		/* ---------------------------------------------------------------------------------------- */
		
		private void EmitImpl(T arg1)
		{
			SignalConnectionBase<T> head = WillEmit();
			SignalConnectionBase<T> p = head;
			while(p != null)
			{
				p._listener(arg1);
				if(!p.stayInList)
				{
					p.Dispose();
				}
				p = p._next;
			}
			DidEmit(head);
		}

		/* ---------------------------------------------------------------------------------------- */
		
		/// <summary>
		/// Whether this signal has at least one listener.
		/// </summary>
		public bool HasListeners()
		{
			return _head != null;
		}
		
		/* ---------------------------------------------------------------------------------------- */
		
		protected SignalConnectionBase<T> ConnectImpl(Action<T> listener, bool prioritize)
		{
			SignalConnectionBase<T> conn = new SignalConnectionBase<T>(this, listener);
			if(Dispatching())
			{
				Defer(delegate() {
					ListAdd(conn, prioritize);
				});
			}
			else
			{
				ListAdd(conn, prioritize);
			}
			return conn;
		}
		
		/* ---------------------------------------------------------------------------------------- */
		
		public void Disconnect(SignalConnectionBase<T> conn)
		{
			if(Dispatching())
			{
				Defer(delegate() {
					ListRemove(conn);
				});
			}
			else
			{
				ListRemove(conn);
			}
		}
		
		/* ---------------------------------------------------------------------------------------- */
		
		protected void Defer(Action fn)
		{
			Task tail = null, p = _deferredTasks;
			while(p != null)
			{
				tail = p;
				p = p.next;
			}
			
			Task task = new Task(fn);
			if(tail != null)
			{
				tail.next = task;
			}
			else
			{
				_deferredTasks = task;
			}
		}
		
		/* ---------------------------------------------------------------------------------------- */
		
		protected SignalConnectionBase<T> WillEmit()
		{
			// Should never happen, since the public emit methods will defer, but just in case...
			if(Dispatching())
			{
				throw new Exception();
			}
			
			SignalConnectionBase<T> snapshot = _head;
			_head = DISPATCHING_SENTINEL;
			return snapshot;
		}
		
		/* ---------------------------------------------------------------------------------------- */
		
		protected void DidEmit(SignalConnectionBase<T> head)
		{
			_head = head;
			
			Task snapshot = _deferredTasks;
			_deferredTasks = null;
			while(snapshot != null)
			{
				snapshot.fn();
				snapshot = snapshot.next;
			}
		}
		
		/* ---------------------------------------------------------------------------------------- */
		
		protected void ListAdd(SignalConnectionBase<T> conn, bool prioritize)
		{
			if(prioritize)
			{
				// Prepend it to the beginning of the list
				conn._next = _head;
				_head = conn;
			}
			else
			{
				// Append it to the end of the list
				SignalConnectionBase<T> tail = null, p = _head;
				while(p != null)
				{
					tail = p;
					p = p._next;
				}
				if(tail != null)
				{
					tail._next = conn;
				}
				else
				{
					_head = conn;
				}
			}
		}
		
		/* ---------------------------------------------------------------------------------------- */
		
		protected void ListRemove(SignalConnectionBase<T> conn)
		{
			SignalConnectionBase<T> prev = null, p = _head;
			while(p != null)
			{
				if(p == conn)
				{
					// Splice out p
					var next = p._next;
					if(prev == null)
					{
						_head = next;
					}
					else
					{
						prev._next = next;
					}
					return;
				}
				prev = p;
				p = p._next;
			}
		}
		
		/* ---------------------------------------------------------------------------------------- */
		
		protected bool Dispatching()
		{
			return _head == DISPATCHING_SENTINEL;
		}
	}


	/// <summary>
	/// 
	/// </summary>
	public class Signal<T, U>
	{
		private static SignalConnectionBase<T, U> DISPATCHING_SENTINEL = new SignalConnectionBase<T, U>(null, null);
		private SignalConnectionBase<T, U> _head;
		private Task _deferredTasks;
		
		/* ---------------------------------------------------------------------------------------- */
		
		public Signal(Action<T, U> listener = null)
		{
			_head = (listener != null) ? new SignalConnectionBase<T, U>(this, listener) : null;
			_deferredTasks = null;
		}
		
		/* ---------------------------------------------------------------------------------------- */
		
		/// <summary>
		/// Connects a listener to this signal.
		/// </summary>
		/// <param name="prioritize">True if this listener should fire before others.</param>
		public SignalConnection Connect(Action<T, U> listener, bool prioritize = false)
		{
			return ConnectImpl(listener, prioritize);
		}
		
		/* ---------------------------------------------------------------------------------------- */
		
		/// <summary>
		/// Emit the signal, notifying each connected listener.
		/// </summary>
		public void Emit(T arg1, U arg2)
		{
			if(Dispatching())
			{
				Defer(delegate() {
					EmitImpl(arg1, arg2);
				});
			}
			else
			{
				EmitImpl(arg1, arg2);
			}
		}
		
		/* ---------------------------------------------------------------------------------------- */
		
		private void EmitImpl(T arg1, U arg2)
		{
			SignalConnectionBase<T, U> head = WillEmit();
			SignalConnectionBase<T, U> p = head;
			while(p != null)
			{
				p._listener(arg1, arg2);
				if(!p.stayInList)
				{
					p.Dispose();
				}
				p = p._next;
			}
			DidEmit(head);
		}

		/* ---------------------------------------------------------------------------------------- */
		
		/// <summary>
		/// Whether this signal has at least one listener.
		/// </summary>
		public bool HasListeners()
		{
			return _head != null;
		}
		
		/* ---------------------------------------------------------------------------------------- */
		
		protected SignalConnectionBase<T, U> ConnectImpl(Action<T, U> listener, bool prioritize)
		{
			SignalConnectionBase<T, U> conn = new SignalConnectionBase<T, U>(this, listener);
			if(Dispatching())
			{
				Defer(delegate() {
					ListAdd(conn, prioritize);
				});
			}
			else
			{
				ListAdd(conn, prioritize);
			}
			return conn;
		}
		
		/* ---------------------------------------------------------------------------------------- */
		
		public void Disconnect(SignalConnectionBase<T, U> conn)
		{
			if(Dispatching())
			{
				Defer(delegate() {
					ListRemove(conn);
				});
			}
			else
			{
				ListRemove(conn);
			}
		}
		
		/* ---------------------------------------------------------------------------------------- */
		
		protected void Defer(Action fn)
		{
			Task tail = null, p = _deferredTasks;
			while(p != null)
			{
				tail = p;
				p = p.next;
			}
			
			Task task = new Task(fn);
			if(tail != null)
			{
				tail.next = task;
			}
			else
			{
				_deferredTasks = task;
			}
		}
		
		/* ---------------------------------------------------------------------------------------- */
		
		protected SignalConnectionBase<T, U> WillEmit()
		{
			// Should never happen, since the public emit methods will defer, but just in case...
			if(Dispatching())
			{
				throw new Exception();
			}
			
			SignalConnectionBase<T, U> snapshot = _head;
			_head = DISPATCHING_SENTINEL;
			return snapshot;
		}
		
		/* ---------------------------------------------------------------------------------------- */
		
		protected void DidEmit(SignalConnectionBase<T, U> head)
		{
			_head = head;
			
			Task snapshot = _deferredTasks;
			_deferredTasks = null;
			while(snapshot != null)
			{
				snapshot.fn();
				snapshot = snapshot.next;
			}
		}
		
		/* ---------------------------------------------------------------------------------------- */
		
		protected void ListAdd(SignalConnectionBase<T, U> conn, bool prioritize)
		{
			if(prioritize)
			{
				// Prepend it to the beginning of the list
				conn._next = _head;
				_head = conn;
			}
			else
			{
				// Append it to the end of the list
				SignalConnectionBase<T, U> tail = null, p = _head;
				while(p != null)
				{
					tail = p;
					p = p._next;
				}
				if(tail != null)
				{
					tail._next = conn;
				}
				else
				{
					_head = conn;
				}
			}
		}
		
		/* ---------------------------------------------------------------------------------------- */
		
		protected void ListRemove(SignalConnectionBase<T, U> conn)
		{
			SignalConnectionBase<T, U> prev = null, p = _head;
			while(p != null)
			{
				if(p == conn)
				{
					// Splice out p
					var next = p._next;
					if(prev == null)
					{
						_head = next;
					}
					else
					{
						prev._next = next;
					}
					return;
				}
				prev = p;
				p = p._next;
			}
		}
		
		/* ---------------------------------------------------------------------------------------- */
		
		protected bool Dispatching()
		{
			return _head == DISPATCHING_SENTINEL;
		}
		
		
		
		
	}

	
	/* ---------------------------------------------------------------------------------------- */
	
	public class Task
	{
		public Action fn;
		public Task next = null;
		
		/* ---------------------------------------------------------------------------------------- */
		
		public Task(Action fn)
		{
			this.fn = fn;
		}
		
		/* ---------------------------------------------------------------------------------------- */
	}

}


