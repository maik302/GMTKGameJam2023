using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionStateManager : MonoBehaviour, IGameStateManager {

    private List<ActionStateEvents> _actionEvents;
    private float _timeBetweenEachAction;

    private void OnEnable() {
        Messenger<LevelConfiguration>.AddListener(GameEvents.InitActionStateEvent, SetUpActionState);
    }

    private void OnDisable() {
        Messenger<LevelConfiguration>.RemoveListener(GameEvents.InitActionStateEvent, SetUpActionState);
    }

    private void OnAwake() {
        _actionEvents = new List<ActionStateEvents>();
    }
    
    public void FinishState() {
        Messenger.Broadcast(GameEvents.FinishGameStateEvent);
    }

    public void StartState() {
        StartCoroutine(MakeActions(_actionEvents));
    }

    private void SetUpActionState(LevelConfiguration levelConfiguration) {
        _actionEvents = levelConfiguration.ActionEvents;
        _timeBetweenEachAction = levelConfiguration.TimeBetweenEachAction;
        StartState();
    }

    private IEnumerator MakeActions(List<ActionStateEvents> actionEvents) {
        void MakeAction(ActionStateEvents actionEvent) {
            // TODO set up sprites and animations for each actionEvent
            Debug.Log($"Action! -> {actionEvent}");
        }

        foreach (var actionEvent in actionEvents) {
            yield return new WaitForSeconds(_timeBetweenEachAction);
            MakeAction(actionEvent);
        }

        FinishState();
    }
}
