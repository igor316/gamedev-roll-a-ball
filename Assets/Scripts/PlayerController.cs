using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
	private Rigidbody rb;

	[SyncVar(hook = "UpdateCountText")]
	public int count;

	public float speed;
	public Text countText;
	public Text winText;

	Instantiation GetMediator () {
		return GameObject.Find ("Common").GetComponent<Instantiation>();
	}

	public override void OnStartLocalPlayer ()
	{
		rb = GetComponent<Rigidbody> ();
		countText = GameObject.Find ("CountText").GetComponent<Text>();
		winText = GameObject.Find ("WinText").GetComponent<Text>();
		count = 0;
		UpdateCountText (count);

		GameObject camera = GameObject.Find ("Main Camera");
		CameraController cc = camera.GetComponent<CameraController> ();
		cc.SetPlayer (gameObject);
		CmdAddPlayer ();
	}

	public void IncCount () {
		count++;
		GetMediator().CheckWinner ();
	}

	[Command]
	void CmdAddPlayer () {
		GetMediator ().AddPlayer (this);
	}

	void FixedUpdate ()
	{
		if (!isLocalPlayer) {
			return;
		}

		float moveVertical = Input.GetAxis ("Vertical");
		float moveHorizontal = Input.GetAxis ("Horizontal");

		rb.AddForce (new Vector3 (moveHorizontal, 0.0f, moveVertical) * speed);
	}

	void UpdateCountText (int newCount)
	{
		if (!isLocalPlayer) {
			return;
		}
		countText.text = "Count: " + newCount.ToString();
	}

	[ClientRpc]
	public void RpcSetStatus (bool isWinner) {
		if (winText == null) {
			winText = GameObject.Find ("WinText").GetComponent<Text>();
		}

		if (isWinner) {
			winText.text = "You Win!";
		} else {
			winText.text = "You Lose!";
		}
	}
}
