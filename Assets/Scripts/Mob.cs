using UnityEngine;
using System.Collections;

public abstract class Mob : MonoBehaviour
{
    [HideInInspector]
    public bool facingRight = true;			// For determining which way the mob is currently facing.
    [HideInInspector]
    public bool jump = false;				// Condition for whether the mob should jump.


    [Tooltip("Amount of force added to move the mob left and right.")]
    public float moveForce = 365f;
    [Tooltip("The fastest the mob can travel in the x axis.")]
    public float maxSpeed = 5f;
    [Tooltip("Array of clips for when the mob jumps.")]
    public AudioClip[] jumpClips;
    [Tooltip("Amount of force added when the mob jumps.")]
    public float jumpForce = 1000f;
    [Tooltip("Array of clips for when the mob taunts.")]
    public AudioClip[] taunts;
    [Tooltip("Chance of a taunt happening.")]
    public float tauntProbability = 50f;
    [Tooltip("Delay for when the taunt should happen.")]
    public float tauntDelay = 1f;

    private int tauntIndex;					// The index of the taunts array indicating the most recent taunt.
    private Transform groundCheck;			// A position marking where to check if the mob is grounded.
    protected bool grounded = false;		// Whether or not the mob is grounded.
    private Animator anim;					// Reference to the mob's animator component.


    public virtual void Awake()
    {
        // Setting up references.
        groundCheck = transform.Find("groundCheck");
        anim = GetComponent<Animator>();
    }

    protected abstract bool ShouldJump();
    protected abstract float GetDesiredHorizontalMovement();

    void Update()
    {
        // The mob is grounded if a linecast to the groundcheck position hits
        // anything on the ground layer.
        grounded = IsGrounded();

        // Start jumping when we should, but don't stop just because we can't
        // start now.
        if(ShouldJump())
        {
            jump = true;
        }
    }

    bool IsGrounded()
    {
        return Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
    }

    void FixedUpdate()
    {
        // Cache the desired motion since we'll use it a lot.
        float h = GetDesiredHorizontalMovement();

        // The Speed animator parameter is set to the absolute value of the
        // horizontal input.
        anim.SetFloat("Speed", Mathf.Abs(h));

        // If the mob is changing direction (h has a different sign to
        // velocity.x) or hasn't reached maxSpeed yet...
        if(h * rigidbody2D.velocity.x < maxSpeed)
        {
            // ... add a force to the mob.
            rigidbody2D.AddForce(Vector2.right * h * moveForce);
        }

        // If the mob's horizontal velocity is greater than the maxSpeed...
        if(Mathf.Abs(rigidbody2D.velocity.x) > maxSpeed)
        {
            // ... set the mob's velocity to the maxSpeed in the x axis.
            rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);
        }

        // If the input is moving in a different direction that our current
        // facing, then flip.
        bool is_moving = h != 0;
        bool has_changed_direction = h < 0 == facingRight;
        if(is_moving && has_changed_direction)
        {
            FlipMovementDirection();
        }

        if(jump)
        {
            HandleJump();
        }
    }

    void HandleJump()
    {
        // Set the Jump animator trigger parameter.
        anim.SetTrigger("Jump");

        // Play a random jump audio clip.
        int i = Random.Range(0, jumpClips.Length);
        AudioSource.PlayClipAtPoint(jumpClips[i], transform.position);

        // Add a vertical force to the mob.
        rigidbody2D.AddForce(new Vector2(0f, jumpForce));

        // Make sure the mob can't jump again until the jump conditions
        // from Update are satisfied.
        jump = false;
    }

    void FlipMovementDirection()
    {
        // Switch the way the mob is labelled as facing.
        facingRight = !facingRight;

        // Multiply the mob's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }


    public IEnumerator Taunt()
    {
        // Check the random chance of taunting.
        float tauntChance = Random.Range(0f, 100f);
        if(tauntChance > tauntProbability)
        {
            // Wait for tauntDelay number of seconds.
            yield return new WaitForSeconds(tauntDelay);

            // If there is no clip currently playing.
            if(!audio.isPlaying)
            {
                // Choose a random, but different taunt.
                tauntIndex = TauntRandom();

                // Play the new taunt.
                audio.clip = taunts[tauntIndex];
                audio.Play();
            }
        }
    }


    int TauntRandom()
    {
        // Choose a random index of the taunts array.
        int i = Random.Range(0, taunts.Length);

        // If it's the same as the previous taunt...
        if(i == tauntIndex)
            // ... try another random taunt.
            return TauntRandom();
        else
            // Otherwise return this index.
            return i;
    }
}

// vim:set et sw=4 ts=4:
