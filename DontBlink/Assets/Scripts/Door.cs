using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	public bool open = false;
	public float DoorOpenAngle = 90f;
	public float DoorCloseAngle = 180f;
	public float Smooth = 2f;
	public PuzzleInteract puzzle;
	AudioSource doorOpenSound;
	public Light doorLight;

	public void ChangeDoorState(){
		if (puzzle == null || puzzle.isSolved()) {
			open = !open;
			doorOpenSound.Play ();
		}
	}
	// Use this for initialization
	void Start () {
		doorOpenSound = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (puzzle != null && !puzzle.isSolved()) {
			if (doorLight != null) {
				doorLight.color = Color.red;
			}
		} else {
			if (doorLight != null) {
				doorLight.color = Color.green;
			}
		}
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
