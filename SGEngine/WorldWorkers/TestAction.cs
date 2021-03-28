using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAction {

    string m;
    int secondStartNotif;
    public bool stop;
    public TestAction(string mrss) {
        m = mrss;
    }
    public void ShowMessage() {
        Debug.Log(m);
        var r = Random.Range(1, 10);
        stop = (r > 7) ? true : false;
    }
}
