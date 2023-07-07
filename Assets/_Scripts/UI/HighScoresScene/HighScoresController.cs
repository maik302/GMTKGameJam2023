using UnityEngine;
using UnityEngine.SceneManagement;
public class HighScoresController : MonoBehaviour {

    void Start() {
        
    }

    public void GoToMainMenuScene() {
        SceneManager.LoadScene(ScenesNames.MainMenuScene);
    }
}
