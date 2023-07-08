using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishKoStateManager : MonoBehaviour, IGameStateManager {
    private void OnEnable() {
        Messenger.AddListener(GameEvents.InitFinishKoStateEvent, StartState);
    }

    private void OnDisable() {
        Messenger.RemoveListener(GameEvents.InitFinishKoStateEvent, StartState);
    }

    public void FinishState(GameStates nextGameState) {
        //TODO
        Debug.Log($"The FINISH_KO state has finished!");
    }

    public void StartState() {
        //TODO
        Debug.Log($"The FINISH_KO state has started!");
    }
}
