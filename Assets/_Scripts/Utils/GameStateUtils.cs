using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStateUtils {

    public static string GetInitGameStateEvent(GameStates gameState) {
        return gameState switch {
            GameStates.START => GameEvents.InitStartStateEvent,
            GameStates.ACTION => GameEvents.InitActionStateEvent,
            GameStates.UPDATE => GameEvents.InitUpdateStateEvent,
            GameStates.FINISH_OK => GameEvents.InitFinishOkStateEvent,
            GameStates.FINISH_KO => GameEvents.InitFinishKoStateEvent,
            _ => GameEvents.InitStartStateEvent
        };
    }
}
