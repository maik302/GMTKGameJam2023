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
        Messenger<GameStates>.AddListener(GameEvents.FinishGameStateEvent, StartNextGameState);
    }

    private void OnDisable() {
        Messenger<GameStates>.RemoveListener(GameEvents.FinishGameStateEvent, StartNextGameState);
    }

    private void Start() {
        this.StartTaskAfter(_secondsBetweenStates, StartNextGameState, GameStates.START);
    }

    private void StartNextGameState(GameStates nextGameState) {
        switch (nextGameState) {
            case GameStates.START:
            case GameStates.ACTION: {
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
