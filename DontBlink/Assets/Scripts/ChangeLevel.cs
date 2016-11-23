using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChangeLevel : MonoBehaviour {

	void OnMouseDown() {
		SceneManager.LoadScene ("darkScene");
	}
}
