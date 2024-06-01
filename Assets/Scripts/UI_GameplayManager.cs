using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UI_GameplayManager : MonoBehaviour
{
    [SerializeField] private GameplayManager gameplayManager;

    [Header("UI Texts")]
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private TMP_Text roundText;
    [SerializeField] private TMP_Text scoreTrackerTest;
    [Space(20)]
    [SerializeField] private bool disappearsNonChoices;
    [Space(9)]
    [Header("UI Player buttons")]
    [SerializeField] private Button rockButtonPL;
    [SerializeField] private Button paperButtonPL;
    [SerializeField] private Button scissorButtonPL;

    [Header("Menu ")]
    [SerializeField] private Button startButton;
    [SerializeField] private Transform overlayMenu;
    [Space(9)]
    [Header("Guest Images")]
    [SerializeField] private Image rockImgGuest;
    [SerializeField] private Image paperImgGuest;
    [SerializeField] private Image scissorImgGuest;

    [Space(9)]
    [SerializeField] private Color32 restoreColorPlayer = Color.white;
    [SerializeField] private Color32 restoreColorGuest = Color.white;

    private ButtonData buttonDataPL = new ButtonData();
    private ButtonData buttonDataGuest = new ButtonData();
    private void OnEnable()
    {
        gameplayManager.OnTurnStarted += OnTurnStarted;
        gameplayManager.OnTurnEnded += OnTurnEnded;
        gameplayManager.OnWin += OnWin;
        gameplayManager.OnRoundTimerRunning += RoundTimerRunning;
        startButton.onClick.AddListener(StartButtonPressed);
    }

    private void StartButtonPressed()
    {
        if (overlayMenu.gameObject.activeSelf) 
            overlayMenu.gameObject.SetActive(false);

        gameplayManager.StartGame();
    }

    private void OnDisable()
    {
        gameplayManager.OnTurnStarted -= OnTurnStarted;
        gameplayManager.OnTurnEnded -= OnTurnEnded;
        gameplayManager.OnWin -= OnWin;
        gameplayManager.OnRoundTimerRunning -= RoundTimerRunning;
        startButton.onClick.RemoveListener(StartButtonPressed);


    }
    private void RoundTimerRunning(float obj)
    {
        statusText.text = obj.ToString("F1");
    }

    private void OnTurnEnded(GamePlayer player1, GamePlayer player2)
    {
        Image img = null;
        //Slide up the Choices + color
        switch (player1.CurrentMove.CurrentMove)
        {
            case MoveType.Rock:
                rockButtonPL.enabled = false;
                img = rockButtonPL.GetComponent<Image>();
                img.color = Color.red;
                if (disappearsNonChoices)
                {
                    paperButtonPL.gameObject.SetActive(false);
                    scissorButtonPL.gameObject.SetActive(false);
                }

                buttonDataPL.button = rockButtonPL;
                buttonDataPL.image = img;
                break;
            case MoveType.Paper:
                paperButtonPL.enabled = false;
                img = paperButtonPL.GetComponent<Image>();
                img.color = Color.red;
                if (disappearsNonChoices)
                {
                    rockButtonPL.gameObject.SetActive(false);
                    scissorButtonPL.gameObject.SetActive(false);
                }
                buttonDataPL.button = paperButtonPL;
                buttonDataPL.image = img;
                break;
            case MoveType.Scissor:
                scissorButtonPL.enabled = false;
                img = scissorButtonPL.GetComponent<Image>();
                img.color = Color.red;
                if (disappearsNonChoices)
                {
                    rockButtonPL.gameObject.SetActive(false);
                    paperButtonPL.gameObject.SetActive(false);
                }
                buttonDataPL.button = scissorButtonPL;
                buttonDataPL.image = img;
                break;
        }

        switch (player2.CurrentMove.CurrentMove)
        {
            case MoveType.Rock:
                img = rockImgGuest;
                img.color = Color.red;
                if (disappearsNonChoices)
                {
                    paperImgGuest.gameObject.SetActive(false);
                    scissorImgGuest.gameObject.SetActive(false);
                }
                buttonDataGuest.button = null;
                buttonDataGuest.image = img;
                break;
            case MoveType.Paper:
                img = paperImgGuest;
                img.color = Color.red;
                if (disappearsNonChoices)
                {
                    rockImgGuest.gameObject.SetActive(false);
                    scissorImgGuest.gameObject.SetActive(false);
                }
                buttonDataGuest.button = null;
                buttonDataGuest.image = img;
                break;
            case MoveType.Scissor:
                img = scissorImgGuest;
                img.color = Color.red;
                if (disappearsNonChoices)
                {
                    rockImgGuest.gameObject.SetActive(false);
                    paperImgGuest.gameObject.SetActive(false);
                }
                buttonDataGuest.button = null;
                buttonDataGuest.image = img;
                break;
        }
    }


    private void OnWin(GamePlayer winner)
    {
        var wintext = winner ? $"{winner.name} Wins!" : "Draw";
        var (p1Score, p2Score) = gameplayManager.PlayerScores;
        scoreTrackerTest.text = $"{p1Score} - {p2Score}" ;
        StartCoroutine(TimedText(wintext, 2));


    }
    private IEnumerator TimedText(string text, float timeUntilWipe)
    {
        statusText.text = text;
        yield return new WaitForSeconds(timeUntilWipe);
        statusText.text = "";

    }
    private void OnTurnStarted(int turnCount)
    {
        if (disappearsNonChoices)
        {
            rockButtonPL.gameObject.SetActive(true);
            paperButtonPL.gameObject.SetActive(true);
            scissorButtonPL.gameObject.SetActive(true);

            rockImgGuest.gameObject.SetActive(true);
            paperImgGuest.gameObject.SetActive(true);
            scissorImgGuest.gameObject.SetActive(true);
        }
            
        if (buttonDataPL.image != null)
        {
            buttonDataPL.image.color = restoreColorPlayer;

            if (buttonDataPL.button != null)
            {
                buttonDataPL.button.enabled = true;
            }

            buttonDataPL.button = null;
            buttonDataPL.image = null;
        }
        if (buttonDataGuest.image != null)
        {
            buttonDataGuest.image.color = restoreColorGuest;

            if (buttonDataGuest.button != null)
            {
                buttonDataGuest.button.enabled = true;
            }

            buttonDataGuest.button = null;
            buttonDataGuest.image = null;
        }
        roundText.text = $"Round {turnCount}";
        StartCoroutine(TimedText("Turn Start", 2));
    }


}

public class ButtonData
{
    public Button button;
    public Image image;
}