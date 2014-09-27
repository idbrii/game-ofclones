using UnityEngine;
using System.Collections;

public class CloneControl : Mob {

	private GameObject player;
	private PlayerControl input;

	public override void Awake()
	{
		base.Awake();

		player = GameObject.Find("hero");
        if (player == null)
        {
            // Player probably died. Clean ourself up.
            Debug.LogWarning("Player is dead/gone? CloneControl will clean up this clone, but something else might be wrong.", this);

            GameObject.Destroy(gameObject);
            return;
        }

		input = player.GetComponent<PlayerControl>();
	}

	protected override bool ShouldJump()
	{
		return input.jump && grounded;
	}

	protected override float GetDesiredHorizontalMovement()
	{
		return input.GetWalkMovement();
	}
}
