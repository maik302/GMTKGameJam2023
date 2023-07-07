using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

    void Start() {
        
    }

    public void GoToMainGameScene() {
        // TODO
    }

    public void GoToHighScoresScene() {
        SceneManager.LoadScene(ScenesNames.HighScoresScene);
    }
}
