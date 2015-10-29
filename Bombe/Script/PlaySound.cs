// Flambe - Rapid game development
// https://github.com/aduros/flambe/blob/master/LICENSE.txt

using UnityEngine;

namespace Bombe
{
    /// <summary>
    /// An action that plays a sound and waits for it to complete.
    /// ```haxe
    /// script.run(new Sequence([
    ///     // Play a sound
    ///     new PlaySound(sound1),
    ///     // Then wait 2 seconds
    ///     new Delay(2),
    ///     // Then play another sound
    ///     new PlaySound(sound2),
    /// ]));
    /// ```
    /// </summary>
	public class PlaySound : IAction, Disposable
	{
		private AudioClip _sound;
		private float _volume;
		private AudioSource _audioSource;
		private bool _paused = false;
		private bool _allowPitchChange = false;

        /// <param name="sound">The sound to play.</param>
        /// <param name="volume">The volume to pass to `AudioSource.play`.</param>
        /// <param name="bAllowPitchChange">True if you want the audio to change with Time.timeScale.</param>
		public PlaySound(AudioClip sound, float volume = 1.0f, bool bAllowPitchChange = false)
		{
			_sound = sound;
			_volume = volume;
			_allowPitchChange = bAllowPitchChange;
		}

		public float Update (float dt, GameObject actor)
		{
			if (_audioSource == null) 
			{
				_audioSource = actor.AddComponent<AudioSource>();
				_audioSource.clip = _sound;
				_audioSource.volume = _volume;
				_audioSource.Play();
			}

			if ( !_paused && dt <= 0 )
			{
				_audioSource.Pause();
				_paused = true;
			}
			else if ( _paused && dt > 0 )
			{
				_audioSource.Play();
				_paused = false;
			}

			if ( !_paused && _allowPitchChange )
			{
				// Adjust the pitch with the timeScale. Not sure if this causes a huge performance hit or not on Update. -Quinn
				_audioSource.pitch = Time.timeScale;
			}

			// If it's done playing, but not because it's paused, be done.
			if (!_audioSource.isPlaying && !_paused) 
			{
				Dispose();
				return 0; // Finished
			}
			return -1; // Keep waiting
		}

		public void Dispose()
		{
			GameObject.Destroy(_audioSource);
			_audioSource = null;
		}

	}
}

