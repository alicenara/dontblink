using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameLogicUI : MonoBehaviour {

	public GameObject GameStartCanvas;
	public GameObject LookEnemyHint;

	private GameObject[] enemies;
	// Use this for initialization
	void Start () {
		StartCoroutine (hideUI (GameStartCanvas, 3.0f)); //Wait 2 seconds then hide UI
	}

	// Update is called once per frame
	void Update () {
		enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		bool showEnemyHint = false;
		foreach (GameObject enemy in enemies) {
			if (!enemy.GetComponent<CheckForObservers> ().IsObserved ()) {
				showEnemyHint = true;
			}
		}
		if (showEnemyHint) {
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
