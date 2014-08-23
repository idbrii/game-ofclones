using UnityEngine;
using System.Collections;

public class RocketHealth : Vitality
{
    protected override void OnAwake()
    {
    }

    protected override void OnTakeDamage(GameObject enemy)
    {
        // Rockets shouldn't take damage.
    }

    // TODO: Doesn't prevent player from shooting herself.
    protected override void OnDeath()
    {
        // Instantiate the explosion and destroy the rocket.
        gameObject.GetComponent<Rocket>().OnExplode();
        Destroy(gameObject);
    }
}
