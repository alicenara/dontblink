using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraPath : MonoBehaviour {

    public List<CameraWaypoint> waypointList;
    public Camera mainCamera;
    public float speed;
    float startTime;
    float distance;
    bool active = false;
    int waypointIndex = 0;
    KeyCode lastKeycode;
    Transform origin;
    Transform destiny;

    void Awake() {
        if (waypointList.Count > 1) {
            mainCamera.transform.position = waypointList[0].transform.position;
            mainCamera.transform.rotation = waypointList[0].transform.rotation;
        }
    }

    void OnDrawGizmos () {
        Gizmos.color = Color.red;
        if (waypointList.Count > 1) {
            int i = 1;
            for(i = 1; i < waypointList.Count; i++) {
                Gizmos.DrawLine(waypointList[i-1].transform.position, waypointList[i].transform.position);
            }
        }
	}
	
	void NextWaypoint (int dir) {
        origin = waypointList[waypointIndex].transform;
        waypointIndex += dir;
        if (waypointIndex < waypointList.Count && waypointIndex >= 0) {
            destiny = waypointList[waypointIndex].transform;
            distance = Vector3.Distance(origin.position, destiny.position);
            startTime = Time.time;
            active = true;
        } else {
            //active = false;
            waypointIndex -= dir;
        }
    }

    // Update is called once per frame
    void Update() {
        if (active) {
            if (Input.GetKeyDown(lastKeycode)) {
                mainCamera.transform.position = destiny.position;
                mainCamera.transform.rotation = destiny.rotation;
                active = false;
            } else {
                float distCovered = (Time.time - startTime) * speed;
                float fracJourney = distCovered / distance;
                mainCamera.transform.position = Vector3.Lerp(origin.position, destiny.position, fracJourney);
                mainCamera.transform.rotation = Quaternion.Lerp(origin.rotation, destiny.rotation, fracJourney);
                if (Vector3.Distance(mainCamera.transform.position, destiny.position) < 0.01f) {
                    active = false;
                }
            }
        } else {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                lastKeycode = KeyCode.LeftArrow;
                NextWaypoint(-1);
            } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
                lastKeycode = KeyCode.RightArrow;
                NextWaypoint(1);
            }
        }
    }
}
