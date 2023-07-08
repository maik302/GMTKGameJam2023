using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {

    [Header("General Game Settings")]
    [SerializeField]
    private TextMeshProUGUI _currentLevelText;

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
        Messenger.AddListener(GameEvents.FinishUpdateStateOkEvent, FinishUpdateStateOkHandler);
        Messenger.AddListener(GameEvents.FinishUpdateStateKoEvent, FinishUpdateStateKoHandler);
    }

    private void OnDisable() {
        Messenger.RemoveListener(GameEvents.FinishGameStateEvent, StartNextGameState);
        Messenger.RemoveListener(GameEvents.FinishUpdateStateOkEvent, FinishUpdateStateOkHandler);
        Messenger.RemoveListener(GameEvents.FinishUpdateStateKoEvent, FinishUpdateStateKoHandler);
    }

    private void Start() {
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

    private void FinishUpdateStateOkHandler() {
        _currentLevelIndex++;
        if (_currentLevelIndex >= _levels.Count) {
            ChangeGameState(GameStates.FINISH_OK);
        } else {
            StartNextGameState();
        }
    }

    private void FinishUpdateStateKoHandler() {
        ChangeGameState(GameStates.FINISH_KO);
    }
}
