using UnityEngine;
using System.Collections;

public class EnemyHealth : Vitality
{
    [Tooltip("A sprite of the enemy when it's dead.")]
    public Sprite deadEnemy;
    [Tooltip("An optional sprite of the enemy when it's damaged.")]
    public Sprite damagedEnemy;
    [Tooltip("An array of audioclips that can play when the enemy dies.")]
    public AudioClip[] deathClips;
    [Tooltip("A prefab of 100 that appears when the enemy dies.")]
    public GameObject hundredPointsUI;
    [Tooltip("A value to give the minimum amount of Torque when dying")]
    public float deathSpinMin = -100f;
    [Tooltip("A value to give the maximum amount of Torque when dying")]
    public float deathSpinMax = 100f;

    private SpriteRenderer ren;			// Reference to the sprite renderer.
    private Score score;				// Reference to the Score script.

    protected override void OnAwake()
    {
        // Setting up the references.
        ren = transform.Find("body").GetComponent<SpriteRenderer>();
        score = GameObject.Find("Score").GetComponent<Score>();
    }

    protected override void OnTakeDamage(GameObject enemy)
    {
        // If the enemy has a damagedEnemy sprite...
        if(damagedEnemy != null)
        {
            // ... set the sprite renderer's sprite to be the damagedEnemy sprite.
            ren.sprite = damagedEnemy;
        }
    }

    protected override void OnDeath()
    {
        // Find all of the sprite renderers on this object and it's children.
        SpriteRenderer[] otherRenderers = GetComponentsInChildren<SpriteRenderer>();

        // Disable all of them sprite renderers.
        foreach(SpriteRenderer s in otherRenderers)
        {
            s.enabled = false;
        }

        // Re-enable the main sprite renderer and set it's sprite to the deadEnemy sprite.
        ren.enabled = true;
        ren.sprite = deadEnemy;

        // Increase the score by 100 points
        score.score += 100;

        // Allow the enemy to rotate and spin it by adding a torque.
        rigidbody2D.fixedAngle = false;
        rigidbody2D.AddTorque(Random.Range(deathSpinMin,deathSpinMax));

        // Find all of the colliders on the gameobject and set them all to be triggers.
        Collider2D[] cols = GetComponents<Collider2D>();
        foreach(Collider2D c in cols)
        {
            c.isTrigger = true;
        }

        // Play a random audioclip from the deathClips array.
        int i = Random.Range(0, deathClips.Length);
        AudioSource.PlayClipAtPoint(deathClips[i], transform.position);

        // Create a vector that is just above the enemy.
        Vector3 scorePos;
        scorePos = transform.position;
        scorePos.y += 1.5f;

        // Instantiate the 100 points prefab at this point.
        Instantiate(hundredPointsUI, scorePos, Quaternion.identity);
    }
}
