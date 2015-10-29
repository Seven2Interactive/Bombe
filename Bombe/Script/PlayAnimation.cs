using UnityEngine;
using System.Collections;

namespace Bombe
{
	public class PlayAnimation : IAction
	{
		// The game object to call the animation on.
		private Animator _animator;
		// The animation name to call on the given game object.
		private string _sAnimationName;
		// The animation layer needed to play the animation.
		private int _nLayerIndex = 0;
		// Has this Action completed?
		private bool _started = false;


		/* ---------------------------------------------------------------------------------------- */

        /// <param name="gameObject">The GameObject with an Animator attached.</param>
        /// <param name="sAnimName">The Animation to play on the given GameObject Animator.</param>
        /// <param name="nLayerIndex">The Animation Layer to play the Animation on.</param>
		public PlayAnimation(Animator animator, string sAnimName, int nLayerIndex = 0)
		{
			_animator = animator;
			_sAnimationName = sAnimName;
			_nLayerIndex = nLayerIndex;
		}

		/* ---------------------------------------------------------------------------------------- */

		public float Update (float dt, GameObject actor)
		{
			if ( !_started )
			{
				_animator.Play(_sAnimationName, _nLayerIndex, 0f);
				_started = true;
			}
			else
			{
				AnimatorStateInfo currState = _animator.GetCurrentAnimatorStateInfo(_nLayerIndex);
				if ( currState.IsName(_sAnimationName) && currState.normalizedTime >= 1.0f )
				{
					_started = false;
					return 0;
				}
			}


			return -1;
		}

		/* ---------------------------------------------------------------------------------------- */

	}
}


