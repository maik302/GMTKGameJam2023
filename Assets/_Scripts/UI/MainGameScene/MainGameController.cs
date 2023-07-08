using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameController : MonoBehaviour {

    public void ResetMainGameScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenuScene() {
        SceneManager.LoadScene(ScenesNames.MainMenuScene);
    }
}
