using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour
{	
	public float damageAmount = 10f;			// The amount of damage to take when enemies touch the player

	Vitality FindVitalityOnVictim(GameObject victim)
	{
		if (victim == gameObject)
		{
			// No self-damage.
			return null;
		}

		Vitality v = victim.gameObject.GetComponent<Vitality>();
		Vitality ours = gameObject.GetComponentInParent<Vitality>();
		if (v == ours)
		{
			// No self-damage to parent hierarchy.
			return null;
		}
		
		return v;
	}

	void OnCollisionEnter2D(Collision2D victim)
	{
		Vitality v = FindVitalityOnVictim(victim.gameObject);

		if(v)
		{
			if (v.IsReadyToTakeDamage()) 
			{
				// Has health to reduce.
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

