using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuManager : MonoBehaviour {

    public void playButton () {
        SceneManager.LoadScene("testingScene");
    }
    
    public void exitGame () {
        Application.Quit();
    }
}
