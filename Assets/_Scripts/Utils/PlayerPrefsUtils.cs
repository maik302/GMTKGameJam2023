using UnityEngine;

public static class PlayerPrefsUtils {

    public static HighScoreHolder GetHighScoreFromPlayerPrefs() {
        return HighScoreHolder.FromJson(PlayerPrefs.GetString(PlayerPrefsKeys.HighScoreKey));
    }

    public static void SaveHighScoreToPlayerPrefs(HighScoreHolder highScoreHolder) {
        var currentHighScore = GetHighScoreFromPlayerPrefs();
        if (currentHighScore != null ) {
            var highestReachedLevel = currentHighScore.GetReachedLevel();
            var givenReachedLevel = highScoreHolder.GetReachedLevel();
            var elapsedTimeIsLowerThanSaved = currentHighScore.GetElapsedTimeInSeconds() >= highScoreHolder.GetElapsedTimeInSeconds();

            if (givenReachedLevel > highestReachedLevel) {
                PlayerPrefs.SetString(PlayerPrefsKeys.HighScoreKey, highScoreHolder.ToJson());
            } else if (givenReachedLevel == highestReachedLevel && elapsedTimeIsLowerThanSaved) {
                PlayerPrefs.SetString(PlayerPrefsKeys.HighScoreKey, highScoreHolder.ToJson());
            }
        } else {
            PlayerPrefs.SetString(PlayerPrefsKeys.HighScoreKey, highScoreHolder.ToJson());
        }
    }
}
