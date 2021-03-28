using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneActor : MonoBehaviour
{
    public SceneController sceneController;
    public string sceneName;

    [HideInInspector]
    public bool IsLoadScene;
    public void LoadScene() {
        if(IsLoadScene)
            sceneController.LoadScene(sceneName);
    }
    public void UnlockScene() {
        IsUnlockScene();
        IsLoadScene = true;
    }
    public void LockScene() {
        IsLockScene();
        IsLoadScene = false;
    }
    public event Action IsUnlockScene;
    public event Action IsLockScene;
}
