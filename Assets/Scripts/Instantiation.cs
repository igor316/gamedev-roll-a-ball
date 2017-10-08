using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Instantiation : NetworkBehaviour {
	public GameObject pickUpPrefab;
	public int pickupsCount;
	public float radius;

	private GameObject[] pickupObjects;
	private List<PlayerController> players;
	private int count = 0;

	public override void OnStartServer () {
		players = new List<PlayerController> ();
		pickupObjects = new GameObject[pickupsCount];
	
		for (int i = 0; i < pickupsCount; i++) {
			float angle =  i * 2 * (float)Math.PI / pickupsCount;
			pickupObjects [i] = Instantiate (pickUpPrefab, new Vector3 ((float)Math.Cos (angle), 0.1f, (float)Math.Sin (angle)) * radius, Quaternion.identity);
			NetworkServer.Spawn (pickupObjects [i]);
		}
	}

	public void AddPlayer(PlayerController pc) {
		players.Add (pc);
	}

	public void CheckWinner () {
		if (++count == pickupsCount) {
			int maxI = 0;
			int i = 0;
			players.ForEach ((p) => {
				if (p.count > players[maxI].count) {
					maxI = i;
				}
				i++;
			});

			players[maxI].RpcSetStatus (true);

			i = 0;
			players.ForEach ((p) => {
				if (i != maxI) {
					players[i].RpcSetStatus (false);
				}
				i++;
			});
		}
	}
}
