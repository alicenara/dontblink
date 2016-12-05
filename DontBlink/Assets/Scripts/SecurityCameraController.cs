using UnityEngine;
using System.Collections;

public class SecurityCameraController : MonoBehaviour {

	// put the first material here.
	public Material material1;
	// put the second material here.
	public Material material2;
	public Camera securitycamera;
	public PuzzleInteract puzzle = null;

	float turnSpeed = 4.0f;		// Speed of camera turning when mouse moves in along an axis
	private Vector3 mouseOrigin;	// Position of cursor when mouse dragging starts
	private bool isRotating;	// Is the camera being rotated?

	bool FirstMaterial = true;
	bool SecondndMaterial = false;
	void Start () 
	{
		
		GetComponent<Renderer>().material = material1;
	}
		

	void Update()
	{
		if (Input.GetKey("o"))
		{
			if (puzzle == null || puzzle.isSolved ()) {
				GetComponent<Renderer> ().material = material2;
				SecondndMaterial = true;
				FirstMaterial = false;
			}
		}

		// Get the left mouse button
		if(SecondndMaterial)
		{
			// Get mouse origin

			Debug.Log(Input.mousePosition);

			isRotating = true;
		}

		// Rotate camera along X and Y axis
		if (isRotating) {
			//Vector3 pos = Input.mousePosition - mouseOrigin ;
			Debug.Log("hello");
		
			if(Input.GetKey("right")){
				securitycamera.transform.parent.parent.parent.Rotate (Vector3.up * turnSpeed);
			}
			else if(Input.GetKey("left")){
				securitycamera.transform.parent.parent.parent.Rotate (Vector3.down * turnSpeed);
			}
//			if(Input.GetKey("l")){
//				securitycamera.transform.Rotate (Vector3.right * turnSpeed);
//			}
//			else if(Input.GetKey("j")){
//				securitycamera.transform.Rotate (Vector3.left * turnSpeed);
//			}

		}

	}

}



