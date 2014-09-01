using UnityEngine;
using System.Collections;

public class HealthPickup : MonoBehaviour
{
    [Tooltip("How much health the crate gives the player.")]
    public float healthBonus;
    [Tooltip("The sound of the crate being collected.")]
    public AudioClip collect;


    private PickupSpawner pickupSpawner;	// Reference to the pickup spawner.


    void Awake()
    {
        // Setting up the references.
        pickupSpawner = GameObject.Find("pickupManager").GetComponent<PickupSpawner>();
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        // If the player enters the trigger zone...
        if(other.CompareTag("Player"))
        {
            // Get a reference to the player health script.
            Vitality playerHealth = other.GetComponent<Vitality>();

            // Increasse the player's health by the health bonus but clamp it at 100.
            playerHealth.health += healthBonus;
            playerHealth.health = Mathf.Clamp(playerHealth.health, 0f, 100f);

            // Update the health bar.
            playerHealth.UpdateHealthBar();

            // Trigger a new delivery.
            pickupSpawner.StartCoroutine(pickupSpawner.DeliverPickup());

            // Play the collection sound.
            AudioSource.PlayClipAtPoint(collect,transform.position);

            // Destroy the crate.
            Destroy(transform.root.gameObject);
        }
    }
}
