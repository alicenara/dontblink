using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	public bool open = false;
	public float DoorOpenAngle = 90f;
	public float DoorCloseAngle = 180f;
	public float Smooth = 2f;

	public void ChangeDoorState(){
		open = !open;
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (open) {
			// open the door
			Quaternion targetRotationOpen = Quaternion.Euler (0, DoorOpenAngle, 0);
			transform.localRotation = Quaternion.Slerp (transform.localRotation, targetRotationOpen, Smooth * Time.deltaTime);
		} else {
			// close the door
			Quaternion targetRotationClose = Quaternion.Euler (0, DoorCloseAngle, 0);
			transform.localRotation = Quaternion.Slerp (transform.localRotation, targetRotationClose, Smooth * Time.deltaTime);
		}
	}
}
