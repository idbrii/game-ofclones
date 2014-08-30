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
		return input.jump && grounded;
	}

	protected override float GetDesiredHorizontalMovement()
	{
		return input.GetWalkMovement();
	}
}
