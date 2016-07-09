using UnityEngine;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour {

	public GameObject camera;
	private string state;
	private Rigidbody rb;
	private int chargeStart;
	// Use this for initialization
	void Start () {
		state = "start";
		rb = GetComponent<Rigidbody> ();
		rb.isKinematic = true;
	}
	
	// Update is called once per frame
	void Update () {
		//
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
		rb.isKinematic = true;
		int frames = Time.frameCount - chargeStart;
		Vector3 theForwardDirection = transform.TransformDirection (Vector3.forward);
		rb.velocity = theForwardDirection * frames;
		state = "fly";
	}
}
