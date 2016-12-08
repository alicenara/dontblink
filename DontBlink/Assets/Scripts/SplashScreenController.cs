using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SplashScreenController : MonoBehaviour {
    CanvasGroup canvasGroup;
    float progress = 0f;
    bool fading = false;
    public float speed = 2f;
    float target = 0f;

    public float Alpha {
        get { return canvasGroup.alpha; }
    }

    public void Fade (float par) {
        if (!fading) {
            fading = true;
            target = par;
            progress = 0f;
        }        
    }

    // Use this for initialization
    void Awake () {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1;
	}
	
	// Update is called once per frame
	void Update () {
        /*if (Input.GetKeyDown(KeyCode.RightArrow)){
            fading = true;
        }*/
        if (fading) {
            progress += Time.deltaTime * speed;
            if (target == 0) {
                canvasGroup.alpha = Mathf.Lerp(1, 0, progress);
            } else {
                canvasGroup.alpha = Mathf.Lerp(0, 1, progress);
            }
            
            if (canvasGroup.alpha == target) 
                fading = false;            
            
        }
	}
}
