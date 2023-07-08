using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishOkStateManager : MonoBehaviour, IGameStateManager {
    private void OnEnable() {
        Messenger.AddListener(GameEvents.InitFinishOkStateEvent, StartState);
    }

    private void OnDisable() {
        Messenger.RemoveListener(GameEvents.InitFinishOkStateEvent, StartState);
    }

    public void FinishState() {
        //TODO
        Debug.Log($"The FINISH_OK state has finished!");
    }

    public void StartState() {
        //TODO
        Debug.Log($"The FINISH_OK state has started!");
    }
}
