using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class HighScoresController : MonoBehaviour {

    [Header("Score text")]
    [SerializeField]
    private GameObject _scoreTextContainer;
    [SerializeField]
    private TextMeshProUGUI _scoreText;
    

    [Header("Empty score text")]
    [SerializeField]
    private GameObject _emptyScoreTextContainer;
    [SerializeField]
    private TextMeshProUGUI _emptyScoreText;

    private void Awake() {
        LoadHighScore();
    }

    private void LoadHighScore() {
        var highScoreHolder = PlayerPrefsUtils.GetHighScoreFromPlayerPrefs();
        if (highScoreHolder != null) {
            _emptyScoreTextContainer.SetActive(false);

            var elapsedTimeText = TimeSpan.FromSeconds(highScoreHolder.GetElapsedTimeInSeconds()).ToString(@"mm\:ss\:ff");
            _scoreText.text = String.Format(GameTexts.FinishKoDescriptionText, highScoreHolder.GetReachedLevel()) + elapsedTimeText;
            _scoreTextContainer.SetActive(true);
        } else {
            _scoreTextContainer.SetActive(false);
            _emptyScoreText.text = GameTexts.NoGamesPlayedText;
            _emptyScoreTextContainer.SetActive(true);
        }
    }

    public void GoToMainMenuScene() {
        SceneManager.LoadScene(ScenesNames.MainMenuScene);
    }
}
