// Port of Flambe classes.
// Flambe - Rapid game development
// https://github.com/aduros/flambe/blob/master/LICENSE.txt
using System;

namespace Bombe
{
	public class Binding : Behavior
	{
		public delegate float BindingFunction(float nValue);
		
		private Value<float> _target;
		private BindingFunction _fn;

		public Binding(Value<float> target, BindingFunction fn = null)
		{
			_target = target;
			_fn = fn;
		}
		
		public float Update(float dt)
		{
			float value = _target._;
			// TODO: Be lazy and only call _fn when the value is changed?
			if (_fn != null)
			{
				return _fn(value);
			}
			else
			{
				return value;
			}
		}
		
		public bool IsComplete()
		{
			return false;
		}
		
	}
}

