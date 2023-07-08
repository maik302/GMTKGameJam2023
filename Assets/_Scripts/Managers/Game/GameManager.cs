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
        Messenger<GameStates>.AddListener(GameEvents.FinishGameStateEvent, HandleFinishedState);
    }

    private void OnDisable() {
        Messenger<GameStates>.RemoveListener(GameEvents.FinishGameStateEvent, HandleFinishedState);
    }

    private void Start() {
        this.StartTaskAfter(_secondsBetweenStates, StartLevel, _levels[0]);
    }

    private void StartLevel(LevelConfiguration levelConfiguration) {
        Messenger<LevelConfiguration>.Broadcast(GameEvents.InitStartStateEvent, levelConfiguration);
    }

    private void HandleFinishedState(GameStates nextGameState) {
        void ChangeGameState(GameStates gameState) {
            _currentGameState = gameState;
            Messenger.Broadcast(GameStateUtils.GetInitGameStateEvent(gameState));
        }

        if (nextGameState != GameStates.START) {
            ChangeGameState(nextGameState);
        }
    }
}
