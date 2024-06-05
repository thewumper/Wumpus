using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WumpusCore.Controller;
using WumpusUnity;

public class VatUI : MonoBehaviour
{
    void Awake()
    {
        // This is a mega placeholder file. Just switch scenes and let trivia deal with it.
        Controller.GlobalController.StartTrivia();
        SceneController.GlobalSceneController.GotoCorrectScene();
    }
}

