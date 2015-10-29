using UnityEngine;
using System.Collections;
using System;

namespace Bombe 
{

	/// Moves an object to a position.
	public class PlaySpine : IAction 
	{

		/// A reference to the spine skeleton animator.
		private SkeletonAnimation _spine;

		/// The timescale to play the spine animation back at.
		private float _timeScale;

		/// The spine track to playback on.
		private int _track;

		/// If the action has started running yet or not.
		private bool _started = false;

		/// The name of the animation to play.
		private string _animationName;

		/* ---------------------------------------------------------------------------------------- */
		      
		public PlaySpine (string name, float timescale = 1.0f, int track = 0)
		{
			Setup (name, timescale, track);
		}

		/* ---------------------------------------------------------------------------------------- */

		public PlaySpine(SkeletonAnimation spine, string name, float timescale = 1.0f, int track = 0)
		{
			_spine = spine;
			Setup (name, timescale, track);
		}      

		/* ---------------------------------------------------------------------------------------- */

		/// <summary>
		/// Setup the specified name, timescale and track for our animation.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="timescale">Timescale.</param>
		/// <param name="track">Track.</param>
		void Setup(string name, float timescale = 1.0f, int track = 0) 
		{
			_track = track;
			_timeScale = timescale;
			_animationName = name;
		}
		
		/* ---------------------------------------------------------------------------------------- */
		      
		public float Update (float dt, GameObject actor)
		{
			if (!_started) 
			{
				_started = true;

				if (_spine == null) 
				{
					_spine = actor.GetComponent<SkeletonAnimation>();
				}

				_spine.state.SetAnimation(_track, _animationName, false);
			}

			Spine.TrackEntry track = _spine.state.GetCurrent(_track);
			if (track.animation.name != _animationName || track.time >= track.endTime) 
			{
				_started = false;

				if (track.animation.name == _animationName) 
				{
					return track.time - track.endTime;
				}

				return 0;
			}

			// Still animating.
			return -1;
		}
		
	}
}
