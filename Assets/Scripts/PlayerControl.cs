using UnityEngine;
using System.Collections;

public class PlayerControl : Mob
{
    // The desired movement of the player.
    [HideInInspector]
    public float walkMovement = 0f;

    protected override bool ShouldJump()
    {
        // If the jump button is pressed and the player is grounded then the
        // player should jump.
        return Input.GetButtonDown("Jump") && grounded;
    }

    protected override float GetDesiredHorizontalMovement()
    {
        walkMovement = Input.GetAxis("Horizontal");
        return walkMovement;
    }

    public float GetWalkMovement()
    {
        return walkMovement;
    }
}

// vim:set et sw=4 ts=4:
