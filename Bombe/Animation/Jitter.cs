//
// Flambe - Rapid game development
// https://github.com/aduros/flambe/blob/master/LICENSE.txt
using UnityEngine;

namespace Bombe
{
	public class Jitter : Behavior
	{
		public float m_base;
		public float strength;
		
		public Jitter(float nBase, float strength)
		{
			this.m_base = nBase;
			this.strength = strength;
		}
		
		public float Update(float dt)
		{
			return m_base + 2 * Random.value * strength - strength;
		}
		
		public bool IsComplete()
		{
			return false;
		}
	}
}
