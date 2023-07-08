using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelConfiguration {

    public int PlayerLives;
    public List<ActionStateEvents> ActionEvents;
    [Range(0.1f, 1.0f)]
    public float TimeBetweenEachAction = 1.0f;

    public LevelConfiguration(List<ActionStateEvents> actionEvents, int playerLives, float timeBetweenEachAction) {
        this.ActionEvents = actionEvents;
        this.PlayerLives = playerLives;
        this.TimeBetweenEachAction = timeBetweenEachAction;
    }

    public LevelConfiguration GetCopy() {
        var actionEventsCopy = new List<ActionStateEvents>();
        this.ActionEvents.ForEach(actionEvent => {
            actionEventsCopy.Add(actionEvent);
        });

        return new LevelConfiguration(actionEventsCopy, PlayerLives, TimeBetweenEachAction);
    }
}
