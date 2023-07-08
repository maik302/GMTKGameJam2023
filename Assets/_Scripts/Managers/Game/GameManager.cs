using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    [Range(0.5f, 5.0f)]
    private float _secondsBetweenStates = 1.0f;

    private GameStates _currentGameState;

    private void Start() {
        this.StartTaskAfter(_secondsBetweenStates, ChangeGameState, GameStates.START);
    }

    private void ChangeGameState(GameStates gameState) {
        _currentGameState = gameState;
        Messenger.Broadcast(GameStateUtils.GetGameStateEvent(gameState));
    }
}
