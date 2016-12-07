using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLogicUI : MonoBehaviour {

	public GameObject GameStartCanvas;
	public GameObject LookEnemyHint;
	public GameObject GameOverCanvas;


	private GameObject[] enemies;
	private bool alive;


	// Use this for initialization
	void Start () {
		StartCoroutine (hideUI (GameStartCanvas, 3.0f)); //Wait 2 seconds then hide UI
		alive = true;
		GameOverCanvas.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		bool showEnemyHint = false;
		foreach (GameObject enemy in enemies) {
			if (!enemy.GetComponent<CheckForObservers> ().IsObserved ()) {
				showEnemyHint = true;
			}
			if (enemy.GetComponent<UnityStandardAssets.Characters.ThirdPerson.basicAI> ().playerAlive == false) {
				alive = false;
			}
		}

		if (alive == false) {
			if (GameObject.FindGameObjectWithTag ("TV") == true) {
				if (GameObject.FindGameObjectWithTag ("TV").GetComponent<SecurityCameraController> ().SecondMaterial == false) {
					GameOverCanvas.SetActive (true);
				}
			} else {
				GameOverCanvas.SetActive (true);
			}

		}else if (showEnemyHint) {
			if (LookEnemyHint.activeSelf == false) {
				LookEnemyHint.SetActive (true);
				StartCoroutine (hideUI (LookEnemyHint, 3.0f));
			}
		}
	}

	IEnumerator hideUI (GameObject guiParentCanvas, float secondsToWait,bool show = false)
	{
		yield return new WaitForSeconds (secondsToWait);
		guiParentCanvas.SetActive (show);
	}
}
