using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelConfiguration {

    public List<ActionStateEvents> ActionEvents;

    public LevelConfiguration(List<ActionStateEvents> actionEvents) {
        this.ActionEvents = actionEvents;
    }

    public LevelConfiguration GetCopy() {
        var actionEventsCopy = new List<ActionStateEvents>();
        this.ActionEvents.ForEach(actionEvent => {
            actionEventsCopy.Add(actionEvent);
        });

        return new LevelConfiguration(actionEventsCopy);
    }
}
