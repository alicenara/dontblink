using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameEnd : MonoBehaviour {

	public CanvasGroup gameOverCanvas;

	bool gameDone = false;
	float gameDoneTimer = 8.0f;

	// Use this for initialization
	void Start () {
		gameOverCanvas.alpha = 0.0f;
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			gameDone = true;
		}
	}

	void Update() {
		if (gameDone) {
			gameOverCanvas.alpha += 0.5f * Time.deltaTime;
			if (gameOverCanvas.alpha > 1.0f) {
				gameOverCanvas.alpha = 1.0f;
			}
			gameDoneTimer -= Time.deltaTime;
			if (gameDoneTimer < 0.0f) {
				SceneManager.LoadScene ("MainMenu");
			}
		}
	}
}
