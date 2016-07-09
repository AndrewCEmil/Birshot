using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;
	private Vector3 offset;
	// Use this for initialization
	void Start () {
		offset = new Vector3 (10, 10, 0);
	}

	void LateUpdate () {
		transform.position = player.transform.position + offset;
	}
}
