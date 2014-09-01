using UnityEngine;
using System.Collections;

public class Explosive : MonoBehaviour
{
    [Tooltip("Radius within which enemies are killed. If zero, there is no bomb blast.")]
    public float bombRadius = 10f;
    [Tooltip("Force that enemies are thrown from the blast.")]
    public float bombForce = 100f;
    [Tooltip("Audioclip of explosion.")]
    public AudioClip boom;
    [Tooltip("Prefab of explosion effect.")]
    public GameObject explosion;
    [Tooltip("Randomize the rotation of the explosion effect.")]
    public bool randomizeRotation = false;


    private ParticleSystem explosionFX;		// Reference to the particle system of the explosion effect.
    private OnExplodeHandler ExplodeHandler;

    void Awake()
    {
        // Setting up references.
        explosionFX = GameObject.FindGameObjectWithTag("ExplosionFX").GetComponent<ParticleSystem>();

        if (ExplodeHandler == null)
        {
            ExplodeHandler = OnExplode;
        }

        GetComponent<Vitality>().Register(null, OnDeath);
    }

    public delegate void OnExplodeHandler();

    protected virtual void OnExplode()
    {
    }

    public void Register(OnExplodeHandler explode)
    {
        ExplodeHandler = explode;
    }

    public void Explode()
    {
        ExplodeHandler();

        if (bombRadius > 0f)
        {
            ApplyBombBlast();
        }

        Quaternion rotation = Quaternion.identity;
        if (randomizeRotation)
        {
            rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        }

        // Instantiate the explosion prefab.
        Instantiate(explosion, transform.position, rotation);

        if (boom != null)
        {
            // Play the explosion sound effect.
            AudioSource.PlayClipAtPoint(boom, transform.position);
        }

        // Destroy the bomb.
        Destroy(gameObject);
    }

    private void ApplyBombBlast()
    {
        // Find all the colliders on the Enemies layer within the bombRadius.
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, bombRadius, 1 << LayerMask.NameToLayer("Enemies"));

        // For each collider...
        foreach(Collider2D en in enemies)
        {
            // Check if it has a rigidbody (since there is only one per enemy, on the parent).
            Rigidbody2D rb = en.rigidbody2D;
            if(rb != null && rb.tag == "Enemy")
            {
                // Find the Enemy script and set the enemy's health to zero.
                rb.gameObject.GetComponent<Vitality>().TakeLethalDamage(gameObject);

                // Find a vector from the bomb to the enemy.
                Vector3 deltaPos = rb.transform.position - transform.position;

                // Apply a force in this direction with a magnitude of bombForce.
                Vector3 force = deltaPos.normalized * bombForce;
                rb.AddForce(force);
            }
        }

        // Set the explosion effect's position to the bomb's position and play the particle system.
        explosionFX.transform.position = transform.position;
        explosionFX.Play();
    }

    public void OnDeath()
    {
        // When we die, we explode.
        GetComponent<Explosive>().Explode();
    }
}
