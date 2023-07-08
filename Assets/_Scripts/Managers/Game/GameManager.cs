using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [Header("Game States configuration")]
    [SerializeField]
    [Range(0.5f, 5.0f)]
    private float _secondsBetweenStates = 1.0f;

    [Header("Levels configurations")]
    [SerializeField]
    private List<LevelConfiguration> _levels;

    private GameStates _currentGameState;

    private void OnEnable() {
        Messenger.AddListener(GameEvents.FinishGameStateEvent, StartNextGameState);
    }

    private void OnDisable() {
        Messenger.RemoveListener(GameEvents.FinishGameStateEvent, StartNextGameState);
    }

    private void Start() {
        _currentGameState = GameStates.INIT;
        this.StartTaskAfter(_secondsBetweenStates, StartNextGameState);
    }

    private void StartNextGameState() {
        GameStates GetNextGameState() {
            // TODO Add new logic for when the game needs to end (when the player loses all of their lives, or all levels are completed)

            return _currentGameState switch {
                GameStates.INIT => GameStates.START,
                GameStates.START => GameStates.ACTION,
                GameStates.ACTION => GameStates.UPDATE,
                _ => GameStates.START
            };
        }

        var nextGameState = GetNextGameState();
        switch (nextGameState) {
            case GameStates.START:
            case GameStates.ACTION:
            case GameStates.UPDATE: {
                ChangeGameState(nextGameState, _levels[0]);
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
}
