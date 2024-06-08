using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WumpusCore.Controller;
using WumpusUnity;
using WumpusCore.HighScoreNS;
using Random = UnityEngine.Random;

public class GameOverUI : MonoBehaviour
{
    private SceneController sceneController;
    private HighScore highScore;


    // Main stuff
    [SerializeField] private GameObject mainUICanvas;
    [SerializeField] private Button showLeaderboardButton;

    // Name change canvas stuff
    [SerializeField] private GameObject submitNameCanvas;
    [SerializeField] private Button nameSubmitButton;
    [SerializeField] private TMP_InputField nameInputField;

    // Leaderboard canvas
    [SerializeField] private GameObject leaderBoardCanvas;
    [SerializeField] private Button closeLeaderboardButton;
    [SerializeField] private TMP_Text leaderboardText;

    private void Start()
    {
        // Unlock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        mainUICanvas.SetActive(false);
        leaderBoardCanvas.SetActive(false);
        submitNameCanvas.SetActive(true);

        nameSubmitButton.onClick.AddListener(SubmitName);
        showLeaderboardButton.onClick.AddListener(ShowLeaderBoard);
        closeLeaderboardButton.onClick.AddListener(CloseLeaderboard);
    }

    public void PlayAgain()
    {
        SetupNewController();
        Controller.GlobalController.StartGame();
        sceneController.GotoCorrectScene();
    }

    public void QuitToMainMenu()
    {
        SetupNewController();
        SceneManager.LoadScene("Main Menu");
    }

    public void SubmitName()
    {
        mainUICanvas.SetActive(true);
        submitNameCanvas.SetActive(false);
        Debug.Log(nameInputField.text);
        highScore = Controller.GlobalController.SaveHighScore(nameInputField.text);
    }

    public void ShowLeaderBoard()
    {
        mainUICanvas.SetActive(false);
        submitNameCanvas.SetActive(false);
        leaderBoardCanvas.SetActive(true);

        String highscoresText = "";
        List<HighScore.StoredHighScore> scores = highScore.GetTopTen();
        foreach (HighScore.StoredHighScore score in scores)
        {
            highscoresText += $"{score.playerName} - {score.score}\n";
        }

        leaderboardText.text = highscoresText;
    }

    public void CloseLeaderboard()
    {
        mainUICanvas.SetActive(true);
        submitNameCanvas.SetActive(false);
        leaderBoardCanvas.SetActive(false);
    }

    public void SetupNewController()
    {
        ushort randomMap = (ushort)Random.Range(0, 4);

        Debug.Log("Loading map #" + randomMap);

        Controller controller = new Controller
            (Application.dataPath + "/Trivia/Questions.json", Application.dataPath + "/Maps", randomMap);
        sceneController = SceneController.GlobalSceneController;
        sceneController.Reinitialize(controller);
    }
}
