using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
public class GameplayManager : MonoBehaviour
{
    [Header("Players")]
    [SerializeField] private GamePlayer playerOne;
    [SerializeField] private GamePlayer playerTwo;

    [Header("Settings")]
    [SerializeField] private float timeTillNextTurn = 4f;
    [SerializeField] private float timeToCheckWinner = 0.24f;
    [SerializeField] private float roundTimer = 3f;

    private int currentTurn = 0;

    public int CurrentTurn => currentTurn;

    private GamePlayer playerOneReady;
    private GamePlayer playerTwoReady;
    private bool timerRunning = true;

    public event Action<int> OnTurnStarted;
    public event Action<float> OnRoundTimerRunning;
    public event Action OnRoundTimerEnded;
    public event Action<GamePlayer, GamePlayer> OnTurnEnded;
    public event Action<GamePlayer> OnWin;

    private int playerOneScore;
    private int playerTwoScore;

    public  (int p1Score, int p2Score) PlayerScores => (playerOneScore, playerTwoScore);

    public void StartGame()
    {
        TurnStart();
    }
    private void TurnStart()
    {
        currentTurn++;
        playerOne.OnPlayerReady += ReadyPlayer;
        playerTwo.OnPlayerReady += ReadyPlayer;
        StartCoroutine(RoundTimer());
    }

    private void ReadyPlayer(GamePlayer player)
    {
        if (player == playerOne)
        {
            playerOne.OnPlayerReady -= ReadyPlayer;
            playerOneReady = playerOne;

        }
        else
        {
            playerTwo.OnPlayerReady -= ReadyPlayer;
            playerTwoReady = playerTwo;
        }

        // if both ready check winner
        if (playerOneReady && playerTwoReady)
        {
            timerRunning = false;
            StartCoroutine(ReadyPlayers());
        }


    }

    private IEnumerator ReadyPlayers()
    {
        OnTurnEnded?.Invoke(playerOne, playerTwo);

        yield return new WaitForSeconds(timeToCheckWinner);
        var winner = CheckMovesGetWinner();
        if (winner != null)
        {
            PlayerWins(winner);
        }
        else
        {
            //Draw
            Draw();
        }

    }

    //todo: add winner score in a cleaner way than inside checking winner method :P
    private GamePlayer CheckMovesGetWinner()
    {
        var playerOneMove = playerOne.CurrentMove.CurrentMove;
        var playerTwoMove = playerTwo.CurrentMove.CurrentMove;

        if (playerOneMove == playerTwoMove)
        {
            return null;
        }
        else if ((playerOneMove == MoveType.Rock && playerTwoMove == MoveType.Scissor) ||
                 (playerOneMove == MoveType.Paper && playerTwoMove == MoveType.Rock) ||
                 (playerOneMove == MoveType.Scissor && playerTwoMove == MoveType.Paper))
        {
            playerOneScore++;
            return playerOne;
        }
        else
        {
            playerTwoScore++;
            return playerTwo;
        }
    }

    private void PlayerWins(GamePlayer player)
    {
        OnWin?.Invoke(player);
        playerOneReady = null;
        playerTwoReady = null;
        StartCoroutine(RestartRound());
    }

    IEnumerator RestartRound()
    {
        yield return new WaitForSeconds(timeTillNextTurn);
        TurnStart();
    }

    IEnumerator RoundTimer()
    {
        timerRunning = true;
        OnTurnStarted?.Invoke(currentTurn);
        var timer = roundTimer;
        OnRoundTimerRunning?.Invoke(timer);
        while (timer > 0 && timerRunning)
        {
            timer -= Time.deltaTime;
            OnRoundTimerRunning?.Invoke(timer);
            yield return null;
        }
        timer = 0;
        OnRoundTimerRunning?.Invoke(timer);
        RoundTimerEnded();
    }
    private void RoundTimerEnded()
    {
        OnRoundTimerEnded?.Invoke();

        if (playerOneReady == null)
        {
            playerOne.MakeMove(DecideRandomMove());
        }
        if (playerTwoReady == null)
        {
            playerTwo.MakeMove(DecideRandomMove());
        }
    }


    public MoveType DecideRandomMove()
    {
        var moves = Enum.GetValues(typeof(MoveType));

        var randomMoveIndex = Random.Range(0, moves.Length);

        return (MoveType)moves.GetValue(randomMoveIndex);
    }


    private void Draw()
    {
        OnWin?.Invoke(null);
        playerOneReady = null;
        playerTwoReady = null;
        StartCoroutine(RestartRound());
    }


}
