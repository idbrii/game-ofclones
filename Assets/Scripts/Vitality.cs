using UnityEngine;
using System.Collections;

public abstract class Vitality : MonoBehaviour
{
    [Tooltip("The owner's starting health amount. Used to determine if we've taken damage.")]
    public float initialHealth = 100f;
    [HideInInspector]
    public float health;
    [Tooltip("How frequently the owner can be damaged.")]
    public float repeatDamagePeriod = 2f;
    [Tooltip("Array of clips to play when the owner is damaged.")]
    public AudioClip[] ouchClips;
    [Tooltip("The force with which the owner is pushed when hurt.")]
    public float hurtForce = 10f;

    private float lastHitTime;					// The time at which the owner was last hit.


    // Child classes can do their wakeup code here. Seems like unity does some
    // reflection magic to avoid "override" and "interfaces".
    protected abstract void OnAwake();

    void Awake()
    {
        health = initialHealth;

        OnAwake();
    }

    public bool IsReadyToTakeDamage()
    {
        return Time.time > lastHitTime + repeatDamagePeriod;
    }

    public bool IsDamaged()
    {
        return health < initialHealth;
    }

    public void Die(Transform killer)
    {
        // Find all of the colliders on the gameobject and set
        // them all to be triggers.
        Collider2D[] cols = GetComponents<Collider2D>();
        foreach(Collider2D c in cols)
        {
            c.isTrigger = true;
        }

        // Move all sprite parts of the owner to the front
        SpriteRenderer[] spr = GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer s in spr)
        {
            s.sortingLayerName = "UI";
        }

        OnDeath();
    }

    protected abstract void OnDeath();
    protected abstract void OnTakeDamage(Transform enemy);

    public void TakeDamage(Transform enemy, float damageAmount)
    {
        lastHitTime = Time.time;

        OnTakeDamage(enemy);

        // Create a vector that's from the enemy to the owner with an upwards
        // boost.
        Vector3 hurtVector = transform.position - enemy.position + Vector3.up * 5f;

        // Add a force to the owner in the direction of the vector and
        // multiply by the hurtForce.
        rigidbody2D.AddForce(hurtVector * hurtForce);

        // Reduce the owner's health by 10.
        health -= damageAmount;

        // Update what the health bar looks like.
        UpdateHealthBar();

        // Play a random clip of the owner getting hurt.
        int i = Random.Range(0, ouchClips.Length);
        AudioSource.PlayClipAtPoint(ouchClips[i], transform.position);
    }

    public abstract void UpdateHealthBar();
}
