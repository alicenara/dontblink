using UnityEngine;
using System.Collections;

public class LightFlicker : MonoBehaviour {
    Light shinyLight;
    public float timeToFade = 5f;
	// Use this for initialization
	void Awake () {
        shinyLight = GetComponent<Light>();	
	}
	
	// Update is called once per frame
	void Update () {
        // shinyLight.intensity = Mathf.Lerp(shinyLight.intensity, 0, timeToFade * Time.deltaTime);
        if (shinyLight.intensity > 0) {
            shinyLight.intensity -= timeToFade * Time.deltaTime;
        }else {
            shinyLight.intensity += timeToFade * 2 * Time.deltaTime;
        }
        
    }
}
