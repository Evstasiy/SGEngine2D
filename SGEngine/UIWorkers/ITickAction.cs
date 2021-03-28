using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITickAction {

    int GetActionId();
    int GetWaitSecond();
    bool isStop();
    void DoJob();

    void DoEndJob();

}
