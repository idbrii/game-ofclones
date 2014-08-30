using UnityEngine;
using System.Collections;

public class CloneControl : Mob {

	private GameObject player;
	private PlayerControl input;

	public override void Awake()
	{
		base.Awake();

		player = GameObject.Find("hero");
		input = player.GetComponent<PlayerControl>();
	}

	protected override bool ShouldJump()
	{
		return input.jump;
	}

	protected override float GetDesiredHorizontalMovement()
	{
		return player.rigidbody2D.velocity.x;
	}
}
