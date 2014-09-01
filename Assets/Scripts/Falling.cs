using UnityEngine;
using System.Collections;

public class Falling : MonoBehaviour
{
	[Tooltip("The AnimTrigger to fire when we hit the ground")]
	public string onGroundAnimTrigger = "Land";
	
    private bool landed = false; // Whether or not the object has landed.
    private Animator anim;       // Reference to the animator component.

	void Awake() {
        anim = transform.root.GetComponent<Animator>();
	}
	
    void OnTriggerEnter2D(Collider2D other)
    {
        // If the crate hits the ground...
        if(other.CompareTag("ground") && !landed)
        {
            // ... set the Land animator trigger parameter.
            anim.SetTrigger(onGroundAnimTrigger);

            transform.parent = null;
            gameObject.AddComponent<Rigidbody2D>();
            landed = true;

			// We landed, so this component is no longer needed. We keep the
			// 'landed' to ensure we're not double-called.
			Destroy(this);
        }
    }
}
