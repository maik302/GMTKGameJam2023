using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelConfiguration {

    public int PlayerLives;
    public List<ActionStateEvents> ActionEvents;

    public LevelConfiguration(List<ActionStateEvents> actionEvents, int playerLives) {
        this.ActionEvents = actionEvents;
        this.PlayerLives = playerLives;
    }

    public LevelConfiguration GetCopy() {
        var actionEventsCopy = new List<ActionStateEvents>();
        this.ActionEvents.ForEach(actionEvent => {
            actionEventsCopy.Add(actionEvent);
        });

        return new LevelConfiguration(actionEventsCopy, PlayerLives);
    }
}
