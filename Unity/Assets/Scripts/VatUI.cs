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
        // I guess we're gonna have it like this forever because I do not have time to fully implement a vat room
        Controller.GlobalController.StartTrivia();
        SceneController.GlobalSceneController.GotoCorrectScene();
    }
}

