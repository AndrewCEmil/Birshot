using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public GameObject camera;
	private string state;
	private Rigidbody rb;
	// Use this for initialization
	void Start () {
		state = "start";
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		//
	}

	void OnEnable(){
		if( Cardboard.SDK == null )
		{
			Cardboard.Create();
		}
		if (Cardboard.SDK != null)
		{
			Cardboard.SDK.OnTrigger += TriggerPulled;
		}
	}

	void OnDisable(){
		Cardboard.SDK.OnTrigger -= this.TriggerPulled;
	}

	void TriggerPulled() {
		if (state == "start") {
			Debug.Log ("state is start");
			Vector3 theForwardDirection = transform.TransformDirection (Vector3.forward);
			rb.velocity = theForwardDirection;
		}
		Debug.Log("The trigger was pulled!");
	}
}
