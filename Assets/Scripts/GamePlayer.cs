using System;
using UnityEngine;

public class GamePlayer : MonoBehaviour
{
    [SerializeField] private PlayerMove currentMove;
    public PlayerMove CurrentMove => currentMove;

    public event Action<GamePlayer> OnPlayerReady;

    public void MakeMoveRequested()
    {
    }

    private void Awake()
    {
        if (currentMove.CurrentPlayer == null)
            currentMove.CurrentPlayer = this;
    }

    public void MakeMove(MoveType move)
    {
        currentMove.CurrentMove = move;
        OnPlayerReady?.Invoke(this);
    }
}

[Serializable]
public class PlayerMove
{
    public GamePlayer CurrentPlayer;
    public MoveType CurrentMove;
    public PlayerMove(GamePlayer pl, MoveType move)
    {
        CurrentMove = move;
        CurrentPlayer = pl;

    }
}
