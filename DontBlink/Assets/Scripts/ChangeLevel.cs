using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChangeLevel : MonoBehaviour {

	public void toSecondLevel() {
		SceneManager.LoadScene ("testingScene");
	}
}
