using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour
{	
	public float damageAmount = 10f;			// The amount of damage to take when enemies touch the player

	void OnCollisionEnter2D(Collision2D victim)
	{
		Vitality v = victim.gameObject.GetComponent<Vitality>();
		// If the collider has a Vitality component ...
		if(v)
		{
			// ... and if the time exceeds the time of the last hit plus the
			// time between hits...
			if (v.IsReadyToTakeDamage()) 
			{
				// ... and if the player still has health...
				if(v.health > 0f)
				{
					v.TakeDamage(gameObject.transform, damageAmount); 
				}
				// If the player doesn't have health, do some stuff, let him
				// fall into the river to reload the level.
				else
				{
					v.Die(gameObject.transform);
				}
			}
		}
	}
}
