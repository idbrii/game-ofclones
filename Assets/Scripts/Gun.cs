using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    [Tooltip("Prefab of the rocket.")]
    public Rigidbody2D rocket;
    [Tooltip("The speed the rocket will fire at.")]
    public float speed = 20f;

    private PlayerControl playerCtrl;		// Reference to the PlayerControl script.
    private Animator anim;					// Reference to the Animator component.


    void Awake()
    {
        // Setting up the references.
        anim = transform.root.gameObject.GetComponent<Animator>();
        playerCtrl = transform.root.GetComponent<PlayerControl>();
    }


    void Update()
    {
        // If the fire button is pressed...
        if(Input.GetButtonDown("Fire1"))
        {
            // ... set the animator Shoot trigger parameter and play the audioclip.
            anim.SetTrigger("Shoot");
            audio.Play();

            float velocity;
            float z_orientation;

            // If the player is facing right...
            if(playerCtrl.facingRight)
            {
                // ... instantiate the rocket facing right and set it's velocity to the right.
                velocity = speed;
                z_orientation = 0f;
            }
            else
            {
                // Otherwise instantiate the rocket facing left and set it's velocity to the left.
                velocity = -speed;
                z_orientation = 180f;
            }
            Rigidbody2D bulletInstance = Instantiate(rocket, transform.position, Quaternion.Euler(new Vector3(0,0,z_orientation))) as Rigidbody2D;
            bulletInstance.velocity = new Vector2(velocity, 0);
        }
    }
}
