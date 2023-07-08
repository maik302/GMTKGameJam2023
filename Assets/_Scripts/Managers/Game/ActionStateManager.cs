using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionStateManager : MonoBehaviour, IGameStateManager {

    [SerializeField]
    [Range(0.5f, 3f)]
    private float _timeBetweenEachAction = 1.0f;

    private List<ActionStateEvents> _actionEvents;

    private void OnEnable() {
        Messenger<LevelConfiguration>.AddListener(GameEvents.InitActionStateEvent, SetUpActionState);
    }

    private void OnDisable() {
        Messenger<LevelConfiguration>.RemoveListener(GameEvents.InitActionStateEvent, SetUpActionState);
    }

    private void OnAwake() {
        _actionEvents = new List<ActionStateEvents>();
    }
    
    public void FinishState(GameStates nextGameState) {
        Messenger<GameStates>.Broadcast(GameEvents.FinishGameStateEvent, nextGameState);
    }

    public void StartState() {
        StartCoroutine(MakeActions(_actionEvents));
    }

    private void SetUpActionState(LevelConfiguration levelConfiguration) {
        _actionEvents = levelConfiguration.ActionEvents;
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

        FinishState(nextGameState: GameStates.UPDATE);
    }
}
