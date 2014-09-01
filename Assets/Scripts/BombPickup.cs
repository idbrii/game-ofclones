using UnityEngine;
using System.Collections;

public class BombPickup : MonoBehaviour
{
    [Tooltip("Sound for when the bomb crate is picked up.")]
    public AudioClip pickupClip;

    void OnTriggerEnter2D(Collider2D other)
    {
        LayBombs bomb_satchel = other.GetComponent<LayBombs>();
        // If a bomb carrier enters the trigger zone...
        if (bomb_satchel != null)
        {
            // ... play the pickup sound effect.
            AudioSource.PlayClipAtPoint(pickupClip, transform.position);

            // Increase their number of bombs.
            ++bomb_satchel.bombCount;

            // Destroy the crate.
            Destroy(transform.root.gameObject);
        }
    }
}
