using UnityEngine;
using System.Collections;

public class CameraWaypoint : MonoBehaviour {
    public bool ending = true;

	// Use this for initialization
	void OnDrawGizmos () {
        if (ending) {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(transform.position, 0.1f);
        }else {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, 0.05f);
        }
        
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 1.5f);
    }
}
