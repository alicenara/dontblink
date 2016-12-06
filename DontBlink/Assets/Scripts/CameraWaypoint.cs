using UnityEngine;
using System.Collections;

public class CameraWaypoint : MonoBehaviour {

	// Use this for initialization
	void OnDrawGizmos () {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, 0.1f);
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 1.5f);
    }
}
