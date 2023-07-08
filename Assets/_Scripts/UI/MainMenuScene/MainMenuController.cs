using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {
    
    public void GoToMainGameScene() {
        SceneManager.LoadScene(ScenesNames.MainGameScene);
    }

    public void GoToHighScoresScene() {
        SceneManager.LoadScene(ScenesNames.HighScoresScene);
    }
}
