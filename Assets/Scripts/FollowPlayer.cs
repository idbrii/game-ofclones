using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour
{
    [Tooltip("The offset at which the Health Bar follows the player.")]
    public Vector3 offset;

    private Transform player;		// Reference to the player.


    void Awake()
    {
        // Setting up the reference.
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // Set the position to the player's position with the offset.
        transform.position = player.position + offset;
    }
}
