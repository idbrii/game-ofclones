using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour
{
    [Tooltip("The amount of damage to deal when hitting an object")]
    public float damageAmount = 10f;
    [Tooltip("Whether dealing damage causes self-destruction.")]
    public bool isKamikaze = false;

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
                v.TakeDamage(gameObject.transform, damageAmount);

                if (isKamikaze)
                {
                    gameObject.GetComponent<Vitality>().TakeLethalDamage(victim.transform);
                }
            }
        }
    }
}

