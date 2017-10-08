using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Spinner : NetworkBehaviour {
	void Update () {
		if (isServer) {
			return;
		}

		transform.Rotate (new Vector3(15, 30, 45) * Time.deltaTime);
	}

	void OnTriggerEnter(Collider other)
	{
		CmdOnTriggerEnter (other.gameObject.GetComponent<NetworkIdentity>().netId);
	}

	[Command]
	void CmdOnTriggerEnter(NetworkInstanceId id) {
		if (gameObject.activeSelf)
		{
			gameObject.SetActive (false);
			RpcSetInactive ();
			GameObject player = NetworkServer.FindLocalObject (id);
			player.GetComponent<PlayerController> ().IncCount ();
		}
	}

	[ClientRpc]
	void RpcSetInactive () {
		gameObject.SetActive (false);
	}
}
