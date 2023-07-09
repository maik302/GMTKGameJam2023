using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class FinishKoStateManager : MonoBehaviour, IGameStateManager {

    [SerializeField]
    private GameObject _finishGameUI;
    [SerializeField]
    private TextMeshProUGUI _finishKoTitleText;
    [SerializeField]
    private TextMeshProUGUI _finishKoDescriptionText;
    [SerializeField]
    private TextMeshProUGUI _elapsedTimeText;

    private float _elapsedTimeInSeconds;
    private int _reachedLevel;

    private void OnEnable() {
        Messenger<float, int>.AddListener(GameEvents.InitFinishKoStateEvent, SetUpFinishKoState);
    }

    private void OnDisable() {
        Messenger<float, int>.RemoveListener(GameEvents.InitFinishKoStateEvent, SetUpFinishKoState);
    }

    private void Awake() {
        _elapsedTimeInSeconds = 0f;
        _reachedLevel = 0;
    }

    public void FinishState() {
        //TODO
        Debug.Log($"The FINISH_KO state has finished!");
    }

    public void StartState() {
        _finishKoTitleText.text = GameTexts.FinishKoTitleText;
        _finishKoDescriptionText.text = String.Format(GameTexts.FinishKoDescriptionText, _reachedLevel);
        _elapsedTimeText.text = TimeSpan.FromSeconds(_elapsedTimeInSeconds).ToString(@"mm\:ss\:ff");
        _finishGameUI.SetActive(true);
        AudioUtils.PlayYouLoseAnnouncement();
        SaveScore();
    }

    private void SetUpFinishKoState(float elapsedTimeInSeconds, int currentLevelIndex) {
        this._elapsedTimeInSeconds = elapsedTimeInSeconds;
        this._reachedLevel = currentLevelIndex + 1;

        StartState();
    }

    private void SaveScore() {
        PlayerPrefsUtils.SaveHighScoreToPlayerPrefs(
            new HighScoreHolder(_elapsedTimeInSeconds, _reachedLevel)
        );
    }
}
