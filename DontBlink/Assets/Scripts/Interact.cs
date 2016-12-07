using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Interact : MonoBehaviour {

	public string interactButton;
	public float interactDistance = 3f;
	public LayerMask interactLayer; //Filter
	public Image interactIcon; // Picture to show if you can interact or not
	public bool isInteracting;
	public GameObject OpenDoorHintCanvas;
	public GameObject OpenCameraHintCanvas;



	// Use this for initialization
	void Start () {
		// set interact icon invisible
		if (interactIcon != null && OpenDoorHintCanvas != null && OpenCameraHintCanvas != null) {
			interactIcon.enabled = false;
			OpenDoorHintCanvas.SetActive(false);
			OpenCameraHintCanvas.SetActive(false);
		}

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
					// show door hint
					if (hit.collider.gameObject.CompareTag ("Door") == true) {
						if (OpenDoorHintCanvas != null) {
							OpenDoorHintCanvas.SetActive(true);
						}
					}
					// show open TV hint
					if (hit.collider.gameObject.CompareTag ("TV") == true) {
						if (OpenCameraHintCanvas != null) {
							OpenCameraHintCanvas.SetActive(true);
						}
					}
					// if press the interact button "E"
					if (Input.GetButtonDown (interactButton)) {
						if (hit.collider.gameObject.CompareTag ("Door") == true) {
							hit.collider.GetComponent<Door> ().ChangeDoorState ();
						}
					}
				}
			}
		} else {
			interactIcon.enabled = false;
			if (OpenCameraHintCanvas != null) {
				OpenDoorHintCanvas.SetActive (false);
			}
			if (OpenCameraHintCanvas != null) {
				OpenCameraHintCanvas.SetActive (false);
			}
		}
	}
}
