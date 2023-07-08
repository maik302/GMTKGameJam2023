using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class FinishOkStateManager : MonoBehaviour, IGameStateManager {

    [SerializeField]
    private GameObject _finishOkUI;
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

    private void OnAwake() {
        _elapsedTimeInSeconds = 0f;
        _reachedLevel = 0;
    }

    public void FinishState() {
        //TODO
        Debug.Log($"The FINISH_OK state has finished!");
    }

    public void StartState() {
        _elapsedTimeText.text = TimeSpan.FromSeconds(_elapsedTimeInSeconds).ToString(@"mm\:ss\:ff");
        _finishOkUI.SetActive(true);
    }

    private void SetUpFinishOkState(float elapsedTimeInSeconds, int currentLevelIndex) {
        this._elapsedTimeInSeconds = elapsedTimeInSeconds;
        this._reachedLevel = currentLevelIndex;

        StartState();
    }
}
