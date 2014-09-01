using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour
{
    [Tooltip("Audioclip of fuse.")]
    public AudioClip fuse;
    public float fuseTime = 1.5f;


    private LayBombs layBombs;				// Reference to the player's LayBombs script.
    private PickupSpawner pickupSpawner;	// Reference to the PickupSpawner script.


    void Awake()
    {
        // Setting up references.
        pickupSpawner = GameObject.Find("pickupManager").GetComponent<PickupSpawner>();
        if(GameObject.FindGameObjectWithTag("Player"))
        {
            layBombs = GameObject.FindGameObjectWithTag("Player").GetComponent<LayBombs>();
        }

        GetComponent<Explosive>().Register(OnExplode);
    }

    void Start()
    {
        // If the bomb has no parent, it has been laid by the player and should detonate.
        if(transform.root == transform)
        {
            StartCoroutine(BombDetonation());
        }
    }


    IEnumerator BombDetonation()
    {
        // Play the fuse audioclip.
        AudioSource.PlayClipAtPoint(fuse, transform.position);

        // Wait for 2 seconds.
        yield return new WaitForSeconds(fuseTime);

        // Explode the bomb.
        GetComponent<Explosive>().Explode();
    }

    public void OnExplode()
    {
        // The player is now free to lay bombs when he has them.
        layBombs.bombLaid = false;

        // Make the pickup spawner start to deliver a new pickup.
        pickupSpawner.StartCoroutine(pickupSpawner.DeliverPickup());
    }
}
