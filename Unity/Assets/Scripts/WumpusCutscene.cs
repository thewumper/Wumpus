using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

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
    
    // Start is called before the first frame update
    void Start()
    {
        isDropping = false;
        isMoving = false;
        wumpusIsLeaving = false;
        wumpusIsJumpscaring = false;
        isRaising = false;
        wumpusStart = Wumpus.transform.position;
        gameRunning = false;
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

        if (BattlePlayerController.ReadyToUnload && gameRunning)
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(battleScene));
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

            gameRunning = false;
        }
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
            wumpusIsJumpscaring = true;
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
        }
    }

    private void UpdateLeave()
    {
        Wumpus.transform.position -= new Vector3(0, leaveSpeed * Time.deltaTime, 0);
        if (Wumpus.transform.position.y < -10)
        {
            SceneManager.LoadScene(winScene);
        }
    }

    private void UpdateJumpscare()
    {
        wumpusJumpscareTime += jumpscareSpeed * Time.deltaTime;
        Wumpus.transform.position = Vector3.Lerp(wumpusStart, PlayerTarget.transform.position, wumpusJumpscareTime);
        if (wumpusJumpscareTime >= jumpscareCutoff)
        {
            wumpusIsJumpscaring = false;
            SceneManager.LoadScene(loseScene);
        }
    }
    
    public void Yes()
    {
        buttons.SetActive(false);
        isDropping = true;
        StartCoroutine(StartDrop());
    }

    public void No()
    {
        buttons.SetActive(false);
        StartCoroutine(GameLoss());
    }

    private IEnumerator GameWin()
    {
        message.gameObject.SetActive(true);
        message.SetText("Congratulations!");
        yield return new WaitForSeconds(1f);
        message.SetText("I have to go now.");
        yield return new WaitForSeconds(1f);
        message.SetText("Stay safe out there.");
        yield return new WaitForSeconds(1f);
        message.gameObject.SetActive(false);
        wumpusIsLeaving = true;
    }

    private IEnumerator GameLoss()
    {
        message.gameObject.SetActive(true);
        message.SetText("That's too bad.");
        yield return new WaitForSeconds(1f);
        message.SetText("Goodbye :(");
        yield return new WaitForSeconds(1f);
        message.gameObject.SetActive(false);
        isRaising = true;
    }
    
    private IEnumerator StartDrop()
    {
        message.SetText("Good luck");
        yield return new WaitForSeconds(1f);
        message.gameObject.SetActive(false);
    }
}
