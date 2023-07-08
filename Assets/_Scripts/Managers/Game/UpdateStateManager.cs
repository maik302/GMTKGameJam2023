using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateStateManager : MonoBehaviour, IGameStateManager {
    private void OnEnable() {
        Messenger.AddListener(GameEvents.InitUpdateStateEvent, StartState);
    }

    private void OnDisable() {
        Messenger.RemoveListener(GameEvents.InitUpdateStateEvent, StartState);
    }

    public void FinishState() {
        //TODO
        Debug.Log($"The UPDATE state has finished!");
    }

    public void StartState() {
        //TODO
        Debug.Log($"The UPDATE state has started!");
    }
}
