using UnityEngine;
using System.Collections;

public class SafeZone : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	void OnTriggerEnter(Collider coll) {
		if (coll.tag == "Player") {
			coll.tag = "Safe Player";
		}
	}

	void OnTriggerExit(Collider coll) {
		if (coll.tag == "Safe Player") {
			coll.tag = "Player";
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
