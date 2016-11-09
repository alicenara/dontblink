using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    // initial variables
    Rigidbody playerBody;
    Camera playerCam;
    public float sensitivity = 10;
    public float constRotX = 60;
    public float speed = 5;
    float rotY = 0f;

    // Awake is called when the object is activated
    void Awake () {
        playerBody = GetComponent<Rigidbody>();
        playerCam = GetComponentInChildren<Camera>();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    float rotX = Input.GetAxis("Mouse X") * sensitivity + transform.localEulerAngles.y;
        rotY += Input.GetAxis("Mouse Y") * sensitivity;
        rotY = Mathf.Clamp(rotY, -constRotX, constRotX);

        if (Input.GetKey(KeyCode.W)) {
            playerBody.velocity = transform.forward * speed;
        } else if (Input.GetKey(KeyCode.A)) {
            playerBody.velocity = -transform.right * speed;
        } else if (Input.GetKey(KeyCode.S)) {
            playerBody.velocity = -transform.forward * speed;
        } else if (Input.GetKey(KeyCode.D)) {
            playerBody.velocity = transform.right * speed;
        } else {
            playerBody.velocity = new Vector3(0, playerBody.velocity.y, 0);
        }

        transform.localEulerAngles = new Vector3(0, rotX, 0);
        playerCam.transform.localEulerAngles = new Vector3(-rotY, 0, 0);
    }
}