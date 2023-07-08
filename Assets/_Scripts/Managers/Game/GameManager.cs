using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    [Range(0.5f, 5.0f)]
    private float _secondsBetweenStates = 1.0f;

    private GameStates _currentGameState;

    private void Start() {
        StartCoroutine(ChangeGameState(GameStates.START));
    }

    private IEnumerator ChangeGameState(GameStates gameState) {
        yield return new WaitForSeconds(_secondsBetweenStates);
        _currentGameState = gameState;
        Messenger.Broadcast(GameStateUtils.GetGameStateEvent(gameState));
    }
}
