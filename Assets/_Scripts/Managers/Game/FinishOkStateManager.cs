using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class FinishOkStateManager : MonoBehaviour, IGameStateManager {

    [SerializeField]
    private GameObject _finishGameUI;
    [SerializeField]
    private TextMeshProUGUI _finishOkTitleText;
    [SerializeField]
    private TextMeshProUGUI _finishOkDescriptionText;
    [SerializeField]
    private TextMeshProUGUI _elapsedTimeText;

    private float _elapsedTimeInSeconds;
    private int _reachedLevel;

    private void OnEnable() {
        Messenger<float, int>.AddListener(GameEvents.InitFinishOkStateEvent, SetUpFinishOkState);
    }

    private void OnDisable() {
        Messenger<float, int>.RemoveListener(GameEvents.InitFinishOkStateEvent, SetUpFinishOkState);
    }

    private void Awake() {
        _elapsedTimeInSeconds = 0f;
        _reachedLevel = 0;
    }

    public void FinishState() {
        //TODO
        Debug.Log($"The FINISH_OK state has finished!");
    }

    public void StartState() {
        _finishOkTitleText.text = GameTexts.FinishOkTitleText;
        _finishOkDescriptionText.text = GameTexts.FinishOkDescriptionText;
        _elapsedTimeText.text = TimeSpan.FromSeconds(_elapsedTimeInSeconds).ToString(@"mm\:ss\:ff");
        _finishGameUI.SetActive(true);
        AudioUtils.PlayYouWinAnnouncement();
        SaveScore();
    }

    private void SetUpFinishOkState(float elapsedTimeInSeconds, int currentLevelIndex) {
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
