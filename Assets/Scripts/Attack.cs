using UnityEngine;
using System.Collections;
using System;

public class Attack : MonoBehaviour
{
    [Tooltip("The amount of damage to deal when hitting an object")]
    public float damageAmount = 10f;
    [Tooltip("Whether dealing damage causes self-destruction")]
    public bool isKamikaze = false;
	[Tooltip("Deals no damage to objects with these tags")]
	public string[] ignoreList;
	[Tooltip("Deals no damage to object that originated attack")]
	public bool ignoreOriginator = true;
	[HideInInspector]
	public GameObject originator; // who caused this attack (never ourself)

    private Vitality ourVitality;

    void Awake()
    {
        ourVitality = gameObject.GetComponentInParent<Vitality>();
    }

	bool IsIgnored(GameObject victim)
	{
		if (ignoreOriginator && victim == originator)
		{
			return true;
		}

		string first = Array.Find(ignoreList, x => victim.CompareTag(x));
		return !string.IsNullOrEmpty(first);
	}

    Vitality FindVitalityOnVictim(GameObject victim)
    {
        if (victim == gameObject || IsIgnored(victim))
        {
            // No self-damage or ignored damage.
            return null;
        }

        Vitality v = victim.gameObject.GetComponent<Vitality>();
        if (v == ourVitality)
        {
            // No self-damage to parent hierarchy.
            return null;
        }

        return v;
    }

    void OnTriggerEnter2D(Collider2D victim)
    {
        OnHitVictim(victim.gameObject);
    }

    void OnCollisionEnter2D(Collision2D victim)
    {
        OnHitVictim(victim.gameObject);
    }

    private void OnHitVictim(GameObject victim)
    {
        Vitality v = FindVitalityOnVictim(victim);

        if(v)
        {
            if (v.IsReadyToTakeDamage())
            {
                v.TakeDamage(gameObject, damageAmount);

                if (isKamikaze)
                {
                    ourVitality.TakeLethalDamage(victim);
                }
            }
        }
    }
}

