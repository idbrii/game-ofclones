using UnityEngine;
using System.Collections;

public class PlayerHealth : Vitality
{
    private PlayerControl playerControl;		// Reference to the PlayerControl script.
    private Animator anim;						// Reference to the Animator on the player

    private SpriteRenderer healthBar;			// Reference to the sprite renderer of the health bar.
    private Vector3 healthScale;				// The local scale of the health bar initially (with full health).

    protected override void OnAwake()
    {
        // Setting up references.
        playerControl = GetComponent<PlayerControl>();
        healthBar = GameObject.Find("HealthBar").GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        // Getting the intial scale of the healthbar (whilst the player has
        // full health).
        healthScale = healthBar.transform.localScale;
    }


    protected override void OnDeath()
    {
        // ... disable user Player Control script
        GetComponent<PlayerControl>().enabled = false;

        // ... disable the Gun script to stop a dead guy shooting a nonexistant bazooka
        GetComponentInChildren<Gun>().enabled = false;

        // ... Trigger the 'Die' animation state
        anim.SetTrigger("Die");
    }

    protected override void OnTakeDamage(GameObject enemy)
    {
        // Make sure the player can't jump.
        playerControl.jump = false;
    }

    public override void UpdateHealthBar()
    {
        // Set the health bar's colour to proportion of the way between green
        // and red based on the player's health.
        healthBar.material.color = Color.Lerp(Color.green, Color.red, 1 - health * 0.01f);

        // Set the scale of the health bar to be proportional to the player's
        // health.
        healthBar.transform.localScale = new Vector3(healthScale.x * health * 0.01f, 1, 1);
    }
}
