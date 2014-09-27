using UnityEngine;
using System.Collections;

public class Enemy : Mob
{
    private Transform attack;			// Reference to the position of the gameobject used for attacks.
    private bool shouldFlip = false;


    public override void Awake()
    {
        base.Awake();

        // Setting up the references.
        attack = transform.Find("attackVolume").transform;
    }

    public override void FixedUpdate()
    {
        // Do our update before parent.

        shouldFlip = false;

        // Create an array of the enemy's attack colliders.
        Collider2D[] attackHits = Physics2D.OverlapPointAll(attack.position, 1);

        // Check each of the colliders.
        foreach(Collider2D c in attackHits)
        {
            // If any of the colliders is an Obstacle...
            if(c.CompareTag("Obstacle"))
            {
                // ... Flip the enemy and stop checking the other colliders.
                shouldFlip = true;
                break;
            }
        }

        base.FixedUpdate();
    }

    protected override bool ShouldJump()
    {
        // enemies never jump
        return false;
    }

    protected override float GetDesiredHorizontalMovement()
    {
        // move forward ...
        float direction = transform.localScale.x;
        if (shouldFlip)
        {
            // ... unless we hit something
            direction *= -1f;
        }
        return direction;
    }
}
