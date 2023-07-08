using UnityEngine;

public static class PlayerPrefsUtils {

    public static HighScoreHolder GetHighScoreFromPlayerPrefs() {
        return HighScoreHolder.FromJson(PlayerPrefs.GetString(PlayerPrefsKeys.HighScoreKey));
    }

    public static void SaveHighScoreToPlayerPrefs(HighScoreHolder highScoreHolder) {
        var currentHighScore = GetHighScoreFromPlayerPrefs();
        if (currentHighScore != null ) {
            if (currentHighScore.GetElapsedTimeInSeconds() >= highScoreHolder.GetElapsedTimeInSeconds()) {
                PlayerPrefs.SetString(PlayerPrefsKeys.HighScoreKey, highScoreHolder.ToJson());
            }
        } else {
            PlayerPrefs.SetString(PlayerPrefsKeys.HighScoreKey, highScoreHolder.ToJson());
        }
    }
}
