using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HighScoreHolder {
    
    [SerializeField]
    private float _elapsedTimeInSeconds;
    [SerializeField]
    private int _reachedLevel;

    public HighScoreHolder(float elapsedTimeInSeconds, int reachedLevel) {
        _elapsedTimeInSeconds = elapsedTimeInSeconds;
        _reachedLevel = reachedLevel;
    }

    public string ToJson() {
        return JsonUtility.ToJson(this);
    }

    public static HighScoreHolder FromJson(string json) {
        return JsonUtility.FromJson<HighScoreHolder>(json);
    }

    public float GetElapsedTimeInSeconds() {
        return _elapsedTimeInSeconds;
    }

    public int GetReachedLevel() {
        return _reachedLevel;
    }
}
