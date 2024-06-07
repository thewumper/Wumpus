using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class WumpusCutscene : MonoBehaviour
{
    [SerializeField] GameObject tv;
    [SerializeField] private string battleScene;
    [SerializeField] private Transform TVTarget;
    [SerializeField] private Transform PlayerTarget;
    
    [SerializeField] private Transform player;
    [SerializeField] private TMP_Text message;
    [SerializeField] private GameObject buttons;
    [SerializeField] private float speed;
    bool isDropping;

    private bool isMoving;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isDropping)
        {
            tv.transform.position = Vector3.Lerp(tv.transform.position,TVTarget.position,speed * Time.deltaTime);
            if (Vector3.Distance(tv.transform.position, TVTarget.position) <= 0.1)
            {
                isDropping = false;
                isMoving = true;
            }
        }

        if (isMoving)
        {
            player.transform.position = Vector3.Lerp(player.transform.position,PlayerTarget.position,speed * Time.deltaTime);
            if (Vector3.Distance(player.transform.position, PlayerTarget.position) <= 1)
            {
                isDropping = false;
                isMoving = false;
                SceneManager.LoadScene(battleScene, LoadSceneMode.Additive);
            }
        }

        if (BattlePlayerController.GameEnded)
        {
            if (BattlePlayerController.Won == true)
            {
                throw new NotImplementedException();
            }

            if (BattlePlayerController.Won == false)
            {
                throw new NotImplementedException();
            }
        }
    }

    public void No()
    {
        buttons.SetActive(false);
        StartCoroutine(GameLoss());
    }

    private IEnumerator GameLoss()
    {
        message.SetText("That's too bad. I really hoped you would play the game");
        yield return new WaitForSeconds(1f);
        message.SetText("Goodbye :(");
        yield return new WaitForSeconds(1f);
        message.gameObject.SetActive(false);
        BattlePlayerController.GameOver();
    }

    public void Yes()
    {
        buttons.SetActive(false);
        isDropping = true;
        StartCoroutine(StartDrop());

    }
    private IEnumerator StartDrop()
    {
        message.SetText("Good luck");
        yield return new WaitForSeconds(1f);
        message.gameObject.SetActive(false);
    }
}
