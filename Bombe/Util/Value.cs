using UnityEngine;
using System.Collections;
using System;

namespace Bombe
{
    /// <summary>
    /// Port of Flambe classes
    /// https://github.com/aduros/flambe/blob/master/LICENSE.txt
    /// </summary>
	public class Value<T>
	{
        /// <summary>
        /// The wrapped value, setting this to a different value will fire the `changed` signal.
        /// </summary>
		virtual public T _
		{
			get
			{
				return _value;
			}

			set
			{
				T oldValue = _value;
				if (!value.Equals(oldValue))
				{
					_value = value;
					if (_changed != null)
					{
						_changed.Emit(value, oldValue);
					}
				}
			}
		}

        /// <summary>
        /// Emitted when the value has changed. The first listener parameter is the new current value,
        /// the second parameter is the old previous value.
        /// </summary>
		public Signal<T, T> changed
		{
			get
			{
				if (_changed == null)
				{
					_changed = new Signal<T, T>();
				}
				return _changed;
			}
			set
			{
			}
		}

        /// <summary>
        /// The local stored value.
        /// </summary>
		protected T _value;
        /// <summary>
        /// The local signal.
        /// </summary>
		private Signal<T, T> _changed;

		/* ---------------------------------------------------------------------------------------- */

		public Value(T value, Action<T, T> listener = null)
		{
			_value = value;
			_changed = null;
			_changed = (listener != null) ? new Signal<T, T>(listener) : null;
		}

		/* ---------------------------------------------------------------------------------------- */

        /// <summary>
        /// Immediately calls a listener with the current value, and again whenever the value changes.
        /// </summary>
		public Disposable Watch (Action<T, T> listener)
		{
			listener(_value, _value);
			return changed.Connect(listener);
		}

		/* ---------------------------------------------------------------------------------------- */

		public string ToString()
		{
			return "" + _value;
		}

		/* ---------------------------------------------------------------------------------------- */
	}
}
