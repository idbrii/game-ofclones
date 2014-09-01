using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Tooltip("The speed the enemy moves at.")]
    public float moveSpeed = 2f;

    private Transform attack;			// Reference to the position of the gameobject used for attacks.


    void Awake()
    {
        // Setting up the references.
        attack = transform.Find("attackVolume").transform;
    }

    void FixedUpdate()
    {
        // Create an array of the enemy's attack colliders.
        Collider2D[] attackHits = Physics2D.OverlapPointAll(attack.position, 1);

        // Check each of the colliders.
        foreach(Collider2D c in attackHits)
        {
            // If any of the colliders is an Obstacle...
            if(c.CompareTag("Obstacle"))
            {
                // ... Flip the enemy and stop checking the other colliders.
                Flip();
                break;
            }
        }

        // Set the enemy's velocity to moveSpeed in the x direction.
        rigidbody2D.velocity = new Vector2(transform.localScale.x * moveSpeed, rigidbody2D.velocity.y);
    }

    public void Flip()
    {
        // Multiply the x component of localScale by -1.
        Vector3 enemyScale = transform.localScale;
        enemyScale.x *= -1;
        transform.localScale = enemyScale;
    }
}
