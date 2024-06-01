public static class HelperExtensions
{
    public static MoveType GetMoveType(this MoveType moveType)
    {
        switch (moveType)
        {
            case MoveType.Rock:
                return MoveType.Rock;
            case MoveType.Paper:
                return MoveType.Paper;
            case MoveType.Scissor:
                return MoveType.Scissor;
        }
        return MoveType.Scissor;
    }
}
