using UnityEngine;
using System.Collections;

public class CloneTrigger : MonoBehaviour
{
	[Tooltip("The object to create when this trigger is touched.")]
	public GameObject archetype;

	[Tooltip("Seconds between subsequent clones.")]
	public float cooldownTime = 1f;

	// When we last created a clone.
	private float lastCloneTime = 0f;
	// The locator of where to create the clone.
	private Transform target;


	void Awake()
	{
		target = transform.Find("spawnLocation");
	}

	public bool IsReadyToClone()
	{
		return Time.time > lastCloneTime + cooldownTime;
	}

	void OnTriggerEnter2D(Collider2D originator)
	{
		if (IsReadyToClone())
		{
			Instantiate(archetype, target.position, target.rotation);
			lastCloneTime = Time.time;
		}
	}
}
