// Port of Flambe classes.
// Flambe - Rapid game development
// https://github.com/aduros/flambe/blob/master/LICENSE.txt


namespace Bombe
{
	public interface Behavior
	{
		float Update(float dt);

		bool IsComplete();
	}
}
