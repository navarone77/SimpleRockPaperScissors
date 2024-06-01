using UnityEngine;
using UnityEngine.UI;


public class UI_GamePlayer : MonoBehaviour
{
    [SerializeField] private GamePlayer player;
    [SerializeField] private GameplayManager gameplayManager;

    [SerializeField] private Button rockButton, paperButton, scissorButton;

    private void OnEnable()
    {
        gameplayManager.OnTurnStarted += OnTurnStarted;
        rockButton.onClick.AddListener(RockButtonPressed);
        paperButton.onClick.AddListener(PaperButtonPressed);
        scissorButton.onClick.AddListener(ScissorButtonPressed);
        player.OnPlayerReady += Player_OnPlayerReady;
    }
    private void OnDisable()
    {
        gameplayManager.OnTurnStarted -= OnTurnStarted;
        rockButton.onClick.RemoveListener(RockButtonPressed);
        paperButton.onClick.RemoveListener(PaperButtonPressed);
        scissorButton.onClick.RemoveListener(ScissorButtonPressed);
        player.OnPlayerReady -= Player_OnPlayerReady;
    }

    private void Player_OnPlayerReady(GamePlayer player)
    {
        switch (player.CurrentMove.CurrentMove)
        {
            case MoveType.Rock:
                paperButton.interactable = false;
                scissorButton.interactable = false;
                break;
            case MoveType.Paper:
                rockButton.interactable = false;
                scissorButton.interactable = false;
                break;
            case MoveType.Scissor:
                rockButton.interactable = false;
                paperButton.interactable = false;
                break;
        }

    }

    private void ToggleButtons(bool toggled = true)
    {
        rockButton.interactable = toggled;
        paperButton.interactable = toggled;
        scissorButton.interactable = toggled;
    }

    private void OnTurnStarted(int turnCount)
    {
        ToggleButtons();
    }

    private void RockButtonPressed()
    {
        player.MakeMove(MoveType.Rock);
        ToggleButtons(false);
    }
    private void PaperButtonPressed()
    {
        player.MakeMove(MoveType.Paper);
        ToggleButtons(false);
    }
    private void ScissorButtonPressed()
    {
        player.MakeMove(MoveType.Scissor);
        ToggleButtons(false);
    }
 
}
