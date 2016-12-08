using UnityEngine;
using System.Collections;

public class LightFlicker : MonoBehaviour {
    Light shinyLight;
    public float timeToFade = 5f;
    public float cheatVar = 0f;
    public float incCheatVar = 1f;
    float firstStop = 3f * 50;
    float stayOff = 2f * 50;
    float secondStop = 3.5f * 50;
	// Use this for initialization
	void Awake () {
        shinyLight = GetComponent<Light>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        // shinyLight.intensity = Mathf.Lerp(shinyLight.intensity, 0, timeToFade * Time.deltaTime);
        //if (shinyLight.intensity > 0) {
        //    shinyLight.intensity -= timeToFade * Time.deltaTime;
        //}else {
        //    shinyLight.intensity += timeToFade * 2 * Time.deltaTime;
        //}
        if (cheatVar == 500) {
            cheatVar = 0;
        }
        if (cheatVar == 0 || cheatVar == firstStop || cheatVar == secondStop) {
            shinyLight.intensity = 0f;
        }
        if (((cheatVar > firstStop + stayOff && cheatVar < secondStop) || cheatVar < firstStop || cheatVar > 150) && shinyLight.intensity < 5) {
            shinyLight.intensity += 1.2f * 2 * Time.deltaTime;
        }
        cheatVar += incCheatVar;
    }
}
