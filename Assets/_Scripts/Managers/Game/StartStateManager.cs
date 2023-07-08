using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartStateManager : MonoBehaviour, IGameStateManager {
    private void OnEnable() {
        Messenger<LevelConfiguration>.AddListener(GameEvents.InitStartStateEvent, SetUpStartState);
    }

    private void OnDisable() {
        Messenger<LevelConfiguration>.RemoveListener(GameEvents.InitStartStateEvent, SetUpStartState);
    }

    public void FinishState(GameStates nextGameState) {
        //TODO
        Debug.Log($"The START state has finished!");
    }

    public void StartState() {
        //TODO
        Debug.Log($"The START state has started!");
    }

    public void SetUpStartState(LevelConfiguration levelConfiguration) {
        var enemiesHP = levelConfiguration.ActionEvents.Count(actionEvent => actionEvent == ActionStateEvents.DO_DAMAGE_TO_ENEMIES);
        var enemiesCount = levelConfiguration.ActionEvents.Count(actionEvent => actionEvent == ActionStateEvents.KILL_ENEMY);

        Debug.Log($"The START state has started!");
        Debug.Log($"There are: {enemiesCount} enemies with {enemiesHP} HP");
        // TODO Set up sprites with this data
    }
}
