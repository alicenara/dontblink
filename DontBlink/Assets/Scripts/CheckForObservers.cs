using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Behaviour model for checking if the object is seen by an enabled camera.
 */
public class CheckForObservers : MonoBehaviour {
	private bool isObserved = false;
	private int lastCalculation = -1;

	// Use this for initialization
	void Start () {
	}

	/**
	 * Determines if the given point is inside the field of the given camera.
	 * This method does not take into consideration that the visibility of this
	 * point may be obscured by another object (like a wall)
	 */
	private bool PointInCameraFOV(Camera camera, Vector3 point) {
		Vector3 viewPos = camera.WorldToViewportPoint (point);
		double x = viewPos.x;
		double y = viewPos.y;
		double z = viewPos.z;
		return x >= 0.0 && x <= 1.0 && y >= 0.0 && y <= 1.0 && z >= 0.0;
	}

	/**
	 * Determines if there is a wall between the two given points.
	 * 
	 * A wall has layerMask 10000000 (binary)
	 */
	private bool WallBetween(Vector3 a, Vector3 b) {
		int layerMask = 1 << 8;
		return Physics.Raycast (a, (b - a).normalized, (b - a).magnitude, layerMask);
	}

	/**
	 * Determines if the given point is observed by one of the (enabled) cameras in the
	 * scene.
	 */
	private bool PointObserved (Vector3 point) {
		foreach (Camera camera in Camera.allCameras) {
			Vector3 cameraPosition = camera.transform.position;
			if (PointInCameraFOV(camera, point) && !WallBetween(cameraPosition, point)) {
				return true;
			}
		}
		return false;
	}

	/**
	 * Returns a in Iterator of all corners of the bounding box of this object.
	 */
	private IEnumerable<Vector3> IterateAllCorners() {
		Bounds bounds = GetComponent<Collider> ().bounds;
		Vector3 c = bounds.center;
		Vector3 e = bounds.extents;

		yield return new Vector3(c.x - e.x, c.y - e.y, c.z - e.z);
		yield return new Vector3(c.x - e.x, c.y - e.y, c.z + e.z);
		yield return new Vector3(c.x - e.x, c.y + e.y, c.z - e.z);
		yield return new Vector3(c.x - e.x, c.y + e.y, c.z + e.z);

		yield return new Vector3(c.x + e.x, c.y - e.y, c.z - e.z);
		yield return new Vector3(c.x + e.x, c.y - e.y, c.z + e.z);
		yield return new Vector3(c.x + e.x, c.y + e.y, c.z - e.z);
		yield return new Vector3(c.x + e.x, c.y + e.y, c.z + e.z);

		yield break;
	}

	/**
	 * Determines if the object is observed by an enabled camera.
	 */
	private bool DetermineIsObserved() {
		// Note (2016-11-08):
		// This is a rather cheap way to determine the visibility of
		// the object. What this method does is to check if any of the corners
		// are visible from any camer. It might be possible that all corners are
		// obscured by other objects in the scene, but parts of the object are
		// visible.
		//
		// I think for now it is better to design the levels to prevent these
		// kind of cases, and ensure that the object is visible only if any of
		// the corners are visible.
		foreach (Vector3 corner in IterateAllCorners()) {
			if (PointObserved (corner)) {
				return true;
			}
		}
		return false;
	}

	/**
	 * Returns true if this object is observed by an enabled camera.
	 */
	public bool IsObserved() {
		if (lastCalculation != Time.frameCount) {
			isObserved = DetermineIsObserved();
			lastCalculation = Time.frameCount;
		}
		return isObserved;
	}
	
	// Update is called once per frame
	void Update () {
		if (IsObserved ()) {
			Debug.Log ("Is Observed");
		}
	}
}
