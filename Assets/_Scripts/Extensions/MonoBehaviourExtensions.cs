using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonoBehaviourExtensions {

    public static void StartTaskAfter<T>(this MonoBehaviour monoBehaviour, float seconds, Func<T> task) {
        monoBehaviour.StartCoroutine(WaitForSecondsToStartTask(seconds, task));
    }

    private static IEnumerator WaitForSecondsToStartTask<T>(float seconds, Func<T> task) {
        yield return new WaitForSeconds(seconds);
        task();
    }
}
