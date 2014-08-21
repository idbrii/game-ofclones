using UnityEngine;
using System.Collections;

public class PlayerControl : Mob
{
    protected override bool ShouldJump()
    {
        // If the jump button is pressed and the player is grounded then the
        // player should jump.
        return Input.GetButtonDown("Jump") && grounded;
    }

    protected override float GetDesiredHorizontalMovement()
    {
        return Input.GetAxis("Horizontal");
    }
}

// vim:set et sw=4 ts=4:
