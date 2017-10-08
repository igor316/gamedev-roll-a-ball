using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	private Vector3 offset;

	public GameObject player;

	void LateUpdate () {
		if (player != null) {
			transform.position = player.transform.position + offset;
		}
	}

	public void SetPlayer (GameObject p) {
		player = p;
		offset = transform.position - player.transform.position;
	}
}
