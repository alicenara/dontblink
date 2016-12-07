using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuManager : MonoBehaviour {

    public void playButton () {
        SceneManager.LoadScene("firstLevelScene");
    }
    
    public void exitGame () {
		SceneManager.LoadScene("MainMenu");
    }
	public void replayFirstLevel(){
		SceneManager.LoadScene("firstLevelScene");
	}
	public void replaySecondLevel(){
		SceneManager.LoadScene("testingScene");
	}

}
