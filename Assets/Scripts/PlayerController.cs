using UnityEngine;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour {

	public GameObject camera;
	public GameObject powerBar;
	private string state;
	private Rigidbody rb;
	private int chargeStart;
	private Sprite powerSprite;
	private SpriteRenderer powerSpriteRenderer;
	// Use this for initialization
	void Start () {
		state = "start";
		rb = GetComponent<Rigidbody> ();
		rb.isKinematic = true;
		powerSprite = powerBar.GetComponent<Sprite> ();
		powerSpriteRenderer = powerBar.GetComponent<SpriteRenderer> ();
		powerSpriteRenderer.enabled = false;
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
		powerSpriteRenderer.enabled = true;
		state = "charge";
	}

	void TransitionChargeToFly() {
		rb.isKinematic = false;
		int frames = Time.frameCount - chargeStart;
		Vector3 theForwardDirection = camera.transform.TransformDirection (Vector3.forward);
		rb.velocity = theForwardDirection * frames;
		state = "fly";
	}
}
