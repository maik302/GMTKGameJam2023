using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartStateManager : MonoBehaviour, IGameStateManager {
    private void OnEnable() {
        Messenger.AddListener(GameEvents.InitStartStateEvent, StartState);
    }

    private void OnDisable() {
        Messenger.RemoveListener(GameEvents.InitStartStateEvent, StartState);
    }

    public void FinishState(GameStates nextGameState) {
        //TODO
        Debug.Log($"The START state has finished!");
    }

    public void StartState() {
        //TODO
        Debug.Log($"The START state has started!");
    }
}
