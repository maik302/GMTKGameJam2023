using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonoBehaviourExtensions {

    public static void StartTaskAfter(this MonoBehaviour monoBehaviour, float seconds, Action task) {
        monoBehaviour.StartCoroutine(WaitForSecondsToStartTask(seconds, task));
    }

    private static IEnumerator WaitForSecondsToStartTask(float seconds, Action task) {
        yield return new WaitForSeconds(seconds);
        task();
    }

    public static void StartTaskAfter<T>(this MonoBehaviour monoBehaviour, float seconds, Action<T> task, T taskArgument) {
        monoBehaviour.StartCoroutine(WaitForSecondsToStartTask(seconds, task, taskArgument));
    }

    private static IEnumerator WaitForSecondsToStartTask<T>(float seconds, Action<T> task, T taskArgument) {
        yield return new WaitForSeconds(seconds);
        task(taskArgument);
    }
}
