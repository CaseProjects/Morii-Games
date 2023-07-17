using MainHandlers;

public class GameObservables
{
    public readonly ReactiveProperty<GameStates> GameState = new();

    public GameObservables()
    {
        GameState.Value = GameStates.IDLE;
    }
}

public enum GameStates
{
    IDLE,
    RUNNING,
    FIGTHING,
    BRIDGE,
    OBSTACLE,
    FAIL,
    STAIR,
    FINISH
}