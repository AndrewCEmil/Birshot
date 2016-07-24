using UnityEngine;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour {

	public GameObject camera;
	private string state;
	private Rigidbody rb;
	private int chargeStart;
	// Use this for initialization
	private SpriteRenderer[] powerUnits;
	private int currentPowerLevel;
	void Start () {
		state = "start";
		rb = GetComponent<Rigidbody> ();
		rb.isKinematic = true;
		powerUnits = new SpriteRenderer[7];
		for (int i = 0; i < 7; i++) {
			powerUnits [i] = GameObject.Find("PB" + i).GetComponent<SpriteRenderer> ();
		}
		clearPowerBar ();
	}
	
	// Update is called once per frame
	void Update () {
		if (state == "charge") {
			setCurrentPowerLevel ();
			updatePowerBar ();
		} else if (state == "fly") {
			if (rb.velocity.magnitude < 1) {
				TransitionFlyToStart ();
			}
		}
	}

	void setCurrentPowerLevel() {
		currentPowerLevel = Mathf.Clamp ((Time.frameCount - chargeStart) / 10, 0, 7);
	}

	void updatePowerBar() {
		for (int i = 0; i < 7; i++) {
			if (i < currentPowerLevel) {
				powerUnits [i].enabled = true;
			} else {
				powerUnits [i].enabled = false;
			}
		}
	}

	void clearPowerBar() {
		for (int i = 0; i < 7; i++) {
			powerUnits [i].enabled = false;
		}
	}

	void OnEnable(){
		if( Cardboard.SDK == null ) {
			Cardboard.Create();
		}
		if (Cardboard.SDK != null) {
			Cardboard.SDK.OnTrigger += TriggerPulled;
		}
	}

	void OnDisable(){
		Cardboard.SDK.OnTrigger -= this.TriggerPulled;
	}

	void TriggerPulled() {
		Debug.Log (state);
		if (state == "start") {
			TransitionStartToCharge ();
		} else if (state == "charge") {
			TransitionChargeToFly ();
		}
		Debug.Log(state);
	}

	void TransitionStartToCharge() {
		chargeStart = Time.frameCount;
		state = "charge";
	}

	void TransitionChargeToFly() {
		rb.isKinematic = false;
		int frames = Time.frameCount - chargeStart;
		Vector3 theForwardDirection = camera.transform.TransformDirection (Vector3.forward);
		rb.velocity = theForwardDirection * ((float)currentPowerLevel * 20f);
		clearPowerBar ();
		state = "fly";
	}

	public void Reset() {
		TransitionFlyToStart();
	}

	void TransitionFlyToStart () {
		rb.isKinematic = false;
		rb.velocity = new Vector3(0, 0, 0);
		state = "start";
	}


	void OnTriggerEnter(Collider other) {
		if (other.CompareTag ("Target")) {
			//Player has been struck
			Handheld.Vibrate ();
			//GAME OVER
			TransitionFlyToStart ();
		}
	}
}
