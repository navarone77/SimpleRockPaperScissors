using System;
using UnityEngine;
using Random = UnityEngine.Random;
public class AI_Player : MonoBehaviour
{
    [SerializeField] private GamePlayer player;

    [SerializeField] private GameplayManager gameplaymanager;


    private void OnEnable()
    {
        gameplaymanager.OnTurnStarted += OnTurnStarted;
    }
    private void OnTurnStarted(int turnCount)
    {
        DecideMove();
    }

    public void DecideMove()
    {
        var moves = Enum.GetValues(typeof(MoveType));

        var randomMoveIndex = Random.Range(0, moves.Length);

        var randomMove = (MoveType)moves.GetValue(randomMoveIndex);

        player.MakeMove(randomMove);
    }


    private void OnDisable()
    {
        gameplaymanager.OnTurnStarted -= OnTurnStarted;
    }
}
