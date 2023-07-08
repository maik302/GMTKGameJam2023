using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStateUtils {

    public static string GetGameStateEvent(GameStates gameState) {
        return gameState switch {
            GameStates.START => GameEvents.InitStartStateEvent,
            _ => GameEvents.InitStartStateEvent
        };
    }
}
