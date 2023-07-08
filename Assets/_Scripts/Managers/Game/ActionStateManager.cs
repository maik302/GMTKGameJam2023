using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionStateManager : MonoBehaviour, IGameStateManager {
    private void OnEnable() {
        Messenger.AddListener(GameEvents.InitActionStateEvent, StartState);
    }

    private void OnDisable() {
        Messenger.RemoveListener(GameEvents.InitActionStateEvent, StartState);
    }
    
    public void FinishState(GameStates nextGameState) {
        //TODO
        Debug.Log($"The ACTION state has finished!");
    }

    public void StartState() {
        //TODO
        Debug.Log($"The ACTION state has started!");
    }
}
