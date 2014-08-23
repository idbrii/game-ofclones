using UnityEngine;
using System.Collections;

public class RocketHealth : Vitality
{
    protected override void OnAwake()
    {
    }

    protected override void OnTakeDamage(Transform enemy)
    {
        // Rockets shouldn't take damage.
    }

    public override void UpdateHealthBar()
    {
    }

    // TODO: Doesn't prevent player from shooting herself.
    protected override void OnDeath()
    {
        // Instantiate the explosion and destroy the rocket.
        gameObject.GetComponent<Rocket>().OnExplode();
        Destroy(gameObject);
    }
}
