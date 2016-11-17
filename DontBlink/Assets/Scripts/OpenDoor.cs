using UnityEngine;
using System.Collections;

public class OpenDoor : MonoBehaviour {

    public float maxRotation = -90f;
    float opening = 0;
    public float degreesPerUpdate = -1f;
    bool canOpenDoor = false;
    bool openingDoor = false;
    Rigidbody door;
    float doorY = 0;

    void Awake() {
        door = GetComponentInParent<Rigidbody>();
        doorY = door.transform.localEulerAngles.y;
    }

    void OnTriggerEnter(Collider other) {
        canOpenDoor = true;
    }

    void OnTriggerStay(Collider other) {
        canOpenDoor = true;
    }

    void OnTriggerExit(Collider other) {
        canOpenDoor = false;
    }

    // Update is called once per frame
    void Update () {
	    if (canOpenDoor && Input.GetKey(KeyCode.E)) {
            openingDoor = true;
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
