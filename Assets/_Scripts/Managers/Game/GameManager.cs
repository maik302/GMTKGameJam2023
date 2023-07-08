using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {

    [Header("General Game Settings")]
    [SerializeField]
    private TextMeshProUGUI _currentLevelText;
    [SerializeField]
    private GameObject _finishGameUI;

    [Header("Game States configuration")]
    [SerializeField]
    [Range(0.5f, 5.0f)]
    private float _secondsBetweenStates = 1.0f;

    [Header("Levels configurations")]
    [SerializeField]
    private List<LevelConfiguration> _levels;

    private GameStates _currentGameState;
    private int _currentLevelIndex;

    private void OnEnable() {
        Messenger.AddListener(GameEvents.FinishGameStateEvent, StartNextGameState);
        Messenger<float>.AddListener(GameEvents.FinishUpdateStateOkEvent, FinishUpdateStateOkHandler);
        Messenger<float>.AddListener(GameEvents.FinishUpdateStateKoEvent, FinishUpdateStateKoHandler);
    }

    private void OnDisable() {
        Messenger.RemoveListener(GameEvents.FinishGameStateEvent, StartNextGameState);
        Messenger<float>.RemoveListener(GameEvents.FinishUpdateStateOkEvent, FinishUpdateStateOkHandler);
        Messenger<float>.RemoveListener(GameEvents.FinishUpdateStateKoEvent, FinishUpdateStateKoHandler);
    }

    private void Start() {
        _finishGameUI.SetActive(false);
        _currentGameState = GameStates.INIT;
        _currentLevelIndex = 0;
        this.StartTaskAfter(_secondsBetweenStates, StartNextGameState);
    }

    private void StartNextGameState() {
        void UpdateCurrentLevelText() {
            _currentLevelText.text = GameTexts.LevelText + (_currentLevelIndex + 1);
        }

        GameStates GetNextGameState() {
            return _currentGameState switch {
                GameStates.INIT => GameStates.START,
                
                // Main loop
                GameStates.START => GameStates.ACTION,
                GameStates.ACTION => GameStates.UPDATE,
                GameStates.UPDATE => GameStates.START,
                
                _ => GameStates.START
            };
        }

        UpdateCurrentLevelText();
        var nextGameState = GetNextGameState();
        switch (nextGameState) {
            case GameStates.START:
            case GameStates.ACTION:
            case GameStates.UPDATE: {
                ChangeGameState(nextGameState, _levels[_currentLevelIndex]);
                break;
            }

            default: {
                ChangeGameState(nextGameState);
                break;
            }
        }
    }

    private void ChangeGameState(GameStates gameState) {
        _currentGameState = gameState;
        Messenger.Broadcast(GameStateUtils.GetInitGameStateEvent(gameState));
    }

    private void ChangeGameState(GameStates gameState, LevelConfiguration levelConfiguration) {
        _currentGameState = gameState;
        Messenger<LevelConfiguration>.Broadcast(GameStateUtils.GetInitGameStateEvent(gameState), levelConfiguration);
    }

    private void FinishUpdateStateOkHandler(float elapsedTimeInSeconds) {
        void ChangeToFinishOkGameState(float elapsedTimeInSeconds, int currentLevelIndex) {
            _currentGameState = GameStates.FINISH_OK;
            Messenger<float, int>.Broadcast(GameStateUtils.GetInitGameStateEvent(GameStates.FINISH_OK), elapsedTimeInSeconds, currentLevelIndex);
        }

        _currentLevelIndex++;
        if (_currentLevelIndex >= _levels.Count) {
            ChangeToFinishOkGameState(elapsedTimeInSeconds, _currentLevelIndex);
        } else {
            StartNextGameState();
        }
    }

    private void FinishUpdateStateKoHandler(float elapsedTimeInSeconds) {
        void ChangeToFinishKoGameState(float elapsedTimeInSeconds, int currentLevelIndex) {
            _currentGameState = GameStates.FINISH_KO;
            Messenger<float, int>.Broadcast(GameStateUtils.GetInitGameStateEvent(GameStates.FINISH_KO), elapsedTimeInSeconds, currentLevelIndex);
        }

        ChangeToFinishKoGameState(elapsedTimeInSeconds, _currentLevelIndex);
    }
}
