using UnityEngine;
using System.Collections;

public class OpenDoor : MonoBehaviour {

    public float maxRotation = -90f;
    float opening = 0;
    public float degreesPerUpdate = -1f;
	public PuzzleInteract puzzle = null;
    bool canOpenDoor = false;
    bool openingDoor = false;
    Rigidbody door;
    float doorY = 0;

    void Awake() {
        door = GetComponentInParent<Rigidbody>();
        doorY = door.transform.localEulerAngles.y;
    }

    void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			canOpenDoor = true;
		}
    }

    void OnTriggerStay(Collider other) {
		if (other.tag == "Player") {
			canOpenDoor = true;
		}
    }

    void OnTriggerExit(Collider other) {
		if (other.tag == "Player") {
			canOpenDoor = false;
		}
    }

    // Update is called once per frame
    void Update () {
	    if (canOpenDoor && Input.GetKeyDown(KeyCode.E)) {
			if (puzzle == null || puzzle.isSolved ()) {
				openingDoor = true;
			}
        }
        if (openingDoor && opening > maxRotation) {
            opening += degreesPerUpdate;
            openingDoor = opening > maxRotation;
        }else if (opening < 0) {
            opening -= degreesPerUpdate;
        }
        door.transform.localEulerAngles = new Vector3(door.transform.localEulerAngles.x, doorY + opening, door.transform.localEulerAngles.z);
	}
}
