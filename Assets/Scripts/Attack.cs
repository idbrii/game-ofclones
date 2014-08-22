using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour
{
    [Tooltip("The amount of damage to take when enemies touch the player")]
    public float damageAmount = 10f;

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
                v.TakeDamage(gameObject.transform, damageAmount);
            }
        }
    }
}

