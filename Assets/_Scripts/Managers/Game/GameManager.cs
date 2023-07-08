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
    private bool _playerHasLivesLeft;

    private void OnAwake() {
        _playerHasLivesLeft = true;
    }

    private void Start() {
        this.StartTaskAfter(_secondsBetweenStates, StartLevel, _levels[0]);
    }

    private void ChangeGameState(GameStates gameState) {
        _currentGameState = gameState;
        Messenger.Broadcast(GameStateUtils.GetGameStateEvent(gameState));
    }

    private void StartLevel(LevelConfiguration levelConfiguration) {
        Messenger<LevelConfiguration>.Broadcast(GameEvents.InitStartStateEvent, levelConfiguration);
    }
}
