// Port of Flambe classes.
// Flambe - Rapid game development
// https://github.com/aduros/flambe/blob/master/LICENSE.txt
using System;

namespace Bombe
{
	/// <summary>
	/// An int value that may be animated over time.
	/// </summary>
	public class AnimatedInt : Value<int>
	{

		private Behavior _behavior = null;

		/// <summary>
		/// The behavior that is currently animating the value, or null if the value is not being
		/// animated.
		/// </summary>
		public Behavior behavior
		{
			get
			{
				return _behavior;
			}

			set
			{
				_behavior = value;
				Update(0);
			}
		}

		override public int _
		{
			set
			{
				_behavior = null;
				base._ = value;
			}
		}

		public AnimatedInt(int value, Action<int, int> listener = null) : base(value, listener)
		{

		}
			

		public void Update(float dt)
		{
			if (_behavior != null)
			{
				base._ = (int)_behavior.Update(dt);
				if (_behavior.IsComplete())
				{
					_behavior = null;
				}
			}
		}

		/// <summary>
		/// Animates between the two given values.
		/// </summary>
		/// <param name="from">The initial value.</param>
		/// <param name="to">The target value.</param>
		/// <param name="seconds">The animation duration, in seconds.</param>
		/// <param name="easing">The easing function to use, defaults to `Ease.linear`.</param>
		public void Animate(int from, int to, float seconds, EaseFunction easing = null)
		{
			easing = easing == null ? Ease.Linear : easing;
			_ = from;
			//			set__(from);
			AnimateTo(to, seconds, easing);
		}

		/// <summary>
		/// Animates between the current value and the given value.
		/// </summary>
		/// <param name="to">The target value.</param>
		/// <param name="seconds">The animation duration, in seconds.</param>
		/// <param name="easing">The easing function to use, defaults to `Ease.linear`.</param>
		public void AnimateTo(float to, float seconds, EaseFunction easing = null)
		{
			easing = easing == null ? Ease.Linear : easing;
			behavior = new Tween(_value, to, seconds, easing);
		}

		/// <summary>
		/// Animates the current value by the given delta.
		/// </summary>
		/// <param name="by">The delta added to the current value to get the target value.</param>
		/// <param name="seconds">The animation duration, in seconds.</param>
		/// <param name="easing">The easing function to use, defaults to `Ease.linear`.</param>
		public void AnimateBy(float by, float seconds, EaseFunction easing = null)
		{
			easing = easing == null ? Ease.Linear : easing;
			behavior = new Tween(_value, _value + by, seconds, easing);
		}

		public void BindTo(Value<float> to, Binding.BindingFunction fn = null)
		{
			behavior = new Binding(to, fn);
		}

		//		private Behavior set_behavior (Behavior behavior)
		//		{
		//			_behavior = behavior;
		//			Update(0);
		//			return behavior;
		//		}

		//		private Behavior get_behavior()
		//		{
		//			return _behavior;
		//		}


	}
}
