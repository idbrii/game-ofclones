using UnityEngine;
using System.Collections;

public class Vitality : MonoBehaviour
{
    [Tooltip("The owner's starting health amount. Used to determine if we've taken damage.")]
    public float initialHealth = 100f;
    [HideInInspector]
    public float health;
    [HideInInspector]
    public bool isDead = false;
    [Tooltip("How frequently the owner can be damaged.")]
    public float repeatDamagePeriod = 2f;
    [Tooltip("Array of clips to play when the owner is damaged.")]
    public AudioClip[] ouchClips;
    [Tooltip("The force with which the owner is pushed when hurt.")]
    public float hurtForce = 10f;

    private float lastHitTime;					// The time at which the owner was last hit.
    private OnTakeDamageHandler DamageHandler;
    private OnDeathHandler DeathHandler;


    // Child classes can do their wakeup code here. Seems like unity does some
    // reflection magic to avoid "override" and "interfaces".
    protected virtual void OnAwake()
    {
    }

    void Awake()
    {
        health = initialHealth;

        OnAwake();

        if (DamageHandler == null)
        {
            DamageHandler = OnTakeDamage;
        }

        if (DeathHandler == null)
        {
            DeathHandler = OnDeath;
        }
    }

    public bool IsReadyToTakeDamage()
    {
        return Time.time > lastHitTime + repeatDamagePeriod;
    }

    public bool IsDamaged()
    {
        return health < initialHealth;
    }

    void Die(GameObject killer)
    {
        isDead = true;
        health = 0f;

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

        DeathHandler();
    }

    public delegate void OnTakeDamageHandler(GameObject enemy);
    public delegate void OnDeathHandler();

    protected virtual void OnDeath()
    {
    }
    protected virtual void OnTakeDamage(GameObject enemy)
    {
    }

    public void Register(OnTakeDamageHandler damage, OnDeathHandler death)
    {
        DamageHandler = damage;
        DeathHandler = death;
    }

    public void TakeLethalDamage(GameObject enemy)
    {
        Die(enemy);
    }

    public void TakeDamage(GameObject enemy, float damageAmount)
    {
        if (isDead)
        {
            // do nothing
        }
        else if (health < damageAmount)
        {
            Die(enemy);
        }
        else
        {
            HandleDamage(enemy, damageAmount);
        }
    }

    void HandleDamage(GameObject enemy, float damageAmount)
    {
        lastHitTime = Time.time;

        DamageHandler(enemy);

        // Create a vector that's from the enemy to the owner with an upwards
        // boost.
        Vector3 hurtVector = transform.position - enemy.transform.position + Vector3.up * 5f;

        // Add a force to the owner in the direction of the vector and
        // multiply by the hurtForce.
        rigidbody2D.AddForce(hurtVector * hurtForce);

        // Reduce the owner's health by 10.
        health -= damageAmount;

        // Update what the health bar looks like.
        UpdateHealthBar();

        // Play a random clip of the owner getting hurt.
        if (ouchClips.Length > 0)
        {
            int i = Random.Range(0, ouchClips.Length);
            AudioSource.PlayClipAtPoint(ouchClips[i], transform.position);
        }
    }

    public virtual void UpdateHealthBar()
    {
        // Default do nothing for objects with no health bar.
    }
}
