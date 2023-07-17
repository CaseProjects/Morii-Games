using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private GameObservables _gameObservables;

    public void Initialize(GameObservables gameObservables)
    {
        _gameObservables = gameObservables;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.TAGS.ENEMY))
        {
            _gameObservables.GameState.Value = GameStates.FIGTHING;
        }

        if (other.CompareTag(Constants.TAGS.BRIDGE))
        {
            _gameObservables.GameState.Value = GameStates.BRIDGE;
        }

        if (other.CompareTag(Constants.TAGS.FINISH))
        {
            _gameObservables.GameState.Value = GameStates.STAIR;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Constants.TAGS.BRIDGE))
        {
            _gameObservables.GameState.Value = GameStates.RUNNING;
        }
    }
}