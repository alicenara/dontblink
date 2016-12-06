using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Interact : MonoBehaviour {

	public string interactButton;
	public float interactDistance = 3f;
	public LayerMask interactLayer; //Filter
	public Image interactIcon; // Picture to show if you can interact or not
	public bool isInteracting;



	// Use this for initialization
	void Start () {
		// set interact icon invisible
		if(interactIcon != null)
		interactIcon.enabled = false;

	}
	
	// Update is called once per frame
	void Update () {
		// shoot a ray
		Ray ray = new Ray (transform.position, transform.forward);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, interactDistance, interactLayer)) {
			// check if we are not interacting
			if (!isInteracting) {
				if (interactIcon != null) {
					interactIcon.enabled = true;
					// if press the interact button
					if (Input.GetButtonDown (interactButton)) {
						if (hit.collider.CompareTag ("Door")) {
							hit.collider.GetComponent<OpenDoor> ().openDoor();
						}
					}
				}
			}
		} else {
			interactIcon.enabled = false;
		}
	}
}
