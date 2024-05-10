using System;
using TMPro;
using UnityEngine;
using WumpusCore.Controller;
using WumpusCore.Trivia;
using WumpusUnity;

public class VatUI : MonoBehaviour
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

    private AskableQuestion question;
    

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
        
        controller.debug = true;
    }

    private void Start()
    { 
        controller.StartTrivia();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        question = controller.GetTriviaQuestion();
        questionText.SetText(question.questionText);
        
        b1Text.SetText(question.choices[0]);
        b2Text.SetText(question.choices[1]);
        b3Text.SetText(question.choices[2]);
        b4Text.SetText(question.choices[3]);
    }

    public void Choose(int choice)
    {
        controller.SubmitTriviaAnswer(choice);
        Debug.Log(controller.GetState());
        sceneController.GotoCorrectScene();
    }
}