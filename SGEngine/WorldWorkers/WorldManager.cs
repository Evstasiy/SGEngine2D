using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour {

    public static WorldManager worldManager = null;

    public GameObject uiManagerObject;
    public UIManager uiManager { get; private set; }

    private TestAction test;
    private TestAction test1;

    void Start(){
        if (worldManager == null)
            worldManager = this;
        else if (worldManager == this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        CheckAllComponents();

        TickRealizationOne tickRealizationOne = new TickRealizationOne();
        TickRealizationTwo tickRealizationTwo = new TickRealizationTwo();
        uiManager.StartTickAction(tickRealizationOne);
        uiManager.StartTickAction(tickRealizationTwo);

    }

    private void CheckAllComponents() {
        try {
            if(uiManagerObject != null)
                uiManager = uiManagerObject.GetComponent<UIManager>();
            else Debug.LogError("CheckAllComponents() - uiManagerObject is NULL!");

        } catch (Exception ex) {
            Debug.LogError("CheckAllComponents() - " + ex.Message + " | " + ex.StackTrace);
        }
    }

    

    // Update is called once per frame
    void Update() {
    }
}

class TickRealizationOne : ITickAction {

    int waitSecond = 1;
    bool stop;

    public void DoEndJob() {
        Debug.Log("End");
    }

    public void DoJob() {
        Debug.Log("DoOne");
        var r = UnityEngine.Random.Range(1, 10);
        stop = (r > 7) ? true : false;
    }

    public int GetActionId() {
        return GetHashCode();
    }

    public int GetWaitSecond() {
        return waitSecond;
    }

    public bool isStop() {
        return stop;
    }

}
class TickRealizationTwo : ITickAction {

    int waitSecond = 3;
    bool stop;

    public void DoEndJob() {
        Debug.Log("EndTwo");
    }

    public void DoJob() {
        Debug.Log("DoTwo");
        var r = UnityEngine.Random.Range(1, 10);
        stop = (r > 7) ? true : false;
    }

    public int GetActionId() {
        return GetHashCode();
    }

    public int GetWaitSecond() {
        return waitSecond;
    }

    public bool isStop() {
        return stop;
    }

}
