using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    private Dictionary<int, Coroutine> allActivityActions = new Dictionary<int, Coroutine>();

    public void StartTickAction(ITickAction tickAction) {
        if (allActivityActions.Count <= 1000) {
            var coroutine = StartCoroutine(StartTickCoroutine(tickAction));
            allActivityActions.Add(tickAction.GetActionId(), coroutine);
        } else {
            Debug.LogWarning("StartTickAction() - list allActivityActions is full, activity id:" + tickAction.GetActionId() + " is not start.");
        }
    }
    public void StopTickAction(int idAction) {
        if (allActivityActions.ContainsKey(idAction)) {
            StopCoroutine(allActivityActions[idAction]);
            allActivityActions.Remove(idAction);
        } else {
            Debug.LogWarning("StopTickAction() - id:" + idAction + " is not in allActivityActions items");
        }
    }

    private IEnumerator StartTickCoroutine(ITickAction tickAction) {
        int actionId = tickAction.GetActionId();
        int waitSecond = tickAction.GetWaitSecond();
        while (!tickAction.isStop()) {
            tickAction.DoJob();
            yield return new WaitForSeconds(waitSecond);
        }
        tickAction.DoEndJob();
        allActivityActions.Remove(actionId);
    }

}
