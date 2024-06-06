using System;
using TMPro;
using UnityEngine;
using WumpusCore.Controller;
using WumpusCore.Trivia;
using WumpusUnity;

public class TriviaUI : MonoBehaviour
{
    private Controller controller;
    private SceneController sceneController;
    
    [SerializeField]
    private TMP_Text questionText;
    
    [SerializeField]
    private TMP_Text b1Text;
    [SerializeField]
    private TMP_Text b2Text;
    [SerializeField]
    private TMP_Text b3Text;
    [SerializeField]
    private TMP_Text b4Text;

    [SerializeField]
    private AudioSource wrongBuzzer;

    private AskableQuestion question;
    private bool lostTrivia = false;
    

    private void Awake()
    {
        // Instantiates the Controller, if there isn't one already.
        try
        {
            controller = Controller.GlobalController;
        }
        catch (NullReferenceException)
        {
            controller = new Controller
                (Application.dataPath + "/Trivia/Questions.json", Application.dataPath + "/Maps", 0);
        }
        sceneController = SceneController.GlobalSceneController;
    }

    private void Start()
    { 
        if (!controller.hasNextTriviaQuestion()) controller.StartTrivia();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        question = controller.GetTriviaQuestion();
        questionText.SetText(question.questionText);
        
        b1Text.SetText(question.choices[0]);
        b2Text.SetText(question.choices[1]);
        b3Text.SetText(question.choices[2]);
        b4Text.SetText(question.choices[3]);
    }

    private void Update()
    {
        ControllerState currentControllerState = controller.GetState();

        if (currentControllerState == ControllerState.GameOver && lostTrivia == false)
        {
            wrongBuzzer.Play();
            lostTrivia = true;
        }
        else if (lostTrivia)
        {
            if (!wrongBuzzer.isPlaying)
            {
                sceneController.GotoCorrectScene();
            }
        }
        else if (currentControllerState != ControllerState.Trivia)
        {
            sceneController.GotoCorrectScene();
        }
    }

    public void Choose(int choice)
    {
        bool correctness = controller.SubmitTriviaAnswer(choice);
        Debug.Log($"You got the trivia question correct: {correctness}");
        Debug.Log(controller.GetState());
        
        question = controller.GetTriviaQuestion();
        questionText.SetText(question.questionText);
        
        b1Text.SetText(question.choices[0]);
        b2Text.SetText(question.choices[1]);
        b3Text.SetText(question.choices[2]);
        b4Text.SetText(question.choices[3]);
    }
}
