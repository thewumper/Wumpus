using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using WumpusCore.Controller;
using WumpusUnity;

public class WumpusCutscene : MonoBehaviour
{
    [SerializeField] GameObject tv;
    [SerializeField] private string battleScene;
    [SerializeField] private string winScene;
    [SerializeField] private string loseScene;
    [SerializeField] private Transform TVTarget;
    [SerializeField] private Transform TVRaiseTarget;
    [SerializeField] private Transform PlayerTarget;
    [SerializeField] private GameObject Wumpus;
    [HideInInspector] private Vector3 wumpusStart;
    
    [SerializeField] private Transform player;
    [SerializeField] private TMP_Text message;
    [SerializeField] private GameObject buttons;
    [SerializeField] private Image cover;
    [SerializeField] private float dialogueWait;
    [SerializeField] private float gameEndWait;
    [FormerlySerializedAs("speed")] [SerializeField] private float startAnimationSpeed;
    [FormerlySerializedAs("wumpusLeaveSpeed")] [SerializeField] private float leaveSpeed;
    [FormerlySerializedAs("wumpusJumpscareSpeed")] [SerializeField] private float jumpscareSpeed;
    [FormerlySerializedAs("wumpusJumpscareCutoff")] [SerializeField] private float jumpscareCutoff;
    private float wumpusJumpscareTime = 0;
    
    private bool isDropping;
    private bool isMoving;
    private bool wumpusIsLeaving;
    private bool wumpusIsJumpscaring;
    private bool isRaising;
    private bool gameRunning;
    private bool endSceneCalled;
    private bool sceneLoaded;

    private Controller controller;

    private void Awake()
    {
        try
        {
            controller = Controller.GlobalController;
        }
        catch (NullReferenceException)
        {
            throw new Exception("Controller not instantiated");
        }
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        isDropping = false;
        isMoving = false;
        wumpusIsLeaving = false;
        wumpusIsJumpscaring = false;
        isRaising = false;
        wumpusStart = Wumpus.transform.position;
        gameRunning = false;
        endSceneCalled = false;
        sceneLoaded = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDropping)
        {
            UpdateDrop();
            return;
        }

        if (isRaising)
        {
            UpdateRaise();
            return;
        }

        if (isMoving)
        {
            UpdateMove();
            return;
        }

        if (wumpusIsLeaving)
        {
            UpdateLeave();
            return;
        }

        if (wumpusIsJumpscaring)
        {
            UpdateJumpscare();
            return;
        }

        // Calls once the moment ReadyToUnload is set to true, then never again
        if (BattlePlayerController.ReadyToUnload && gameRunning && !isRaising && sceneLoaded)
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(battleScene));
            sceneLoaded = false;
            StartCoroutine(WaitThenExecute(gameEndWait, () => isRaising = true));
            return;
        }

        // Calls once raising completes and never again
        if (BattlePlayerController.ReadyToUnload && !gameRunning && !endSceneCalled)
        {
            endSceneCalled = true;
            
            if (BattlePlayerController.Won == true)
            {
                StartCoroutine(GameWin());
            }

            if (BattlePlayerController.Won == false)
            {
                StartCoroutine(GameLoss());
            }

            if (BattlePlayerController.Won == null)
            {
                throw new InvalidDataException("Cannot tell if game is complete");
            }
        }
    }

    private IEnumerator WaitThenExecute(float time, Action method)
    {
        yield return new WaitForSeconds(time);
        method();
    }

    private void UpdateDrop()
    {
        tv.transform.position = Vector3.Lerp(tv.transform.position,TVTarget.position,startAnimationSpeed * Time.deltaTime);
        if (Vector3.Distance(tv.transform.position, TVTarget.position) <= 0.1)
        {
            isDropping = false;
            isMoving = true;
        }
    }
    
    private void UpdateRaise()
    {
        tv.transform.position = Vector3.Lerp(tv.transform.position,TVRaiseTarget.position,startAnimationSpeed * Time.deltaTime);
        if (Vector3.Distance(tv.transform.position, TVRaiseTarget.position) <= 0.1)
        {
            isRaising = false;
            gameRunning = false;
        }
    }

    private void UpdateMove()
    {
        player.transform.position = Vector3.Lerp(player.transform.position,PlayerTarget.position,startAnimationSpeed * Time.deltaTime);
        if (Vector3.Distance(player.transform.position, PlayerTarget.position) <= 1)
        {
            isDropping = false;
            isMoving = false;
            gameRunning = true;
            SceneManager.LoadScene(battleScene, LoadSceneMode.Additive);
            sceneLoaded = true;
        }
    }

    private void UpdateLeave()
    {
        Wumpus.transform.position -= new Vector3(0, leaveSpeed * Time.deltaTime, 0);
        if (Wumpus.transform.position.y < -2)
        {
            StartCoroutine(WaitThenExecute(gameEndWait, () => SceneController.GlobalSceneController.GotoCorrectScene()));
        }
    }

    private void UpdateJumpscare()
    {
        wumpusJumpscareTime += jumpscareSpeed * Time.deltaTime;
        Wumpus.transform.position = Vector3.Lerp(wumpusStart, PlayerTarget.transform.position, wumpusJumpscareTime);
        if (wumpusJumpscareTime >= jumpscareCutoff)
        {
            wumpusIsJumpscaring = false;
            cover.enabled = true;
            StartCoroutine(WaitThenExecute(gameEndWait, () => SceneController.GlobalSceneController.GotoCorrectScene()));
        }
    }

    private void lockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    public void Yes()
    {
        buttons.SetActive(false);
        lockCursor();
        isDropping = true;
        StartCoroutine(StartDrop());
    }

    public void No()
    {
        buttons.SetActive(false);
        lockCursor();
        StartCoroutine(GameLoss());
    }

    private IEnumerator GameWin()
    {
        message.gameObject.SetActive(true);
        message.SetText("Congratulations!");
        yield return new WaitForSeconds(dialogueWait);
        message.SetText("I have to go now.");
        yield return new WaitForSeconds(dialogueWait);
        message.SetText("Stay safe out there.");
        yield return new WaitForSeconds(dialogueWait);
        message.gameObject.SetActive(false);
        wumpusIsLeaving = true;
        controller.ExitWumpusFight(true);
    }

    private IEnumerator GameLoss()
    {
        message.gameObject.SetActive(true);
        message.SetText("That's too bad.");
        yield return new WaitForSeconds(dialogueWait);
        message.SetText("Goodbye :(");
        yield return new WaitForSeconds(dialogueWait);
        message.gameObject.SetActive(false);
        wumpusIsJumpscaring = true;
        controller.ExitWumpusFight(false);
    }
    
    private IEnumerator StartDrop()
    {
        message.SetText("Good luck");
        yield return new WaitForSeconds(dialogueWait);
        message.gameObject.SetActive(false);
    }
}
