// Port of Flambe classes.
// Flambe - Rapid game development
// https://github.com/aduros/flambe/blob/master/LICENSE.txt
using UnityEngine;


namespace Bombe
{
    /// <summary>
    /// Represents a unit of execution that is called over time.
    /// </summary>
	public interface IAction
	{
        /// <summary>
        /// Called when the acting entity has been updated.
        /// </summary>
        /// <param name="dt">The time elapsed since the last frame, in seconds.</param>
        /// <param name="actor">The entity of the Script that this action was added to.</param>
		float Update(float dt, GameObject actor);
	}
}