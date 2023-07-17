using Core.SignalBus;
using Events;
using Helpers.Extensions;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMoveHandler _playerMoveHandler;
    private PlayerChildRegistry _playerChildRegistry;
    private GameObservables _gameObservables;
    private PlayerFacade _playerFacade;
    private SignalBus _signalBus;


    public void Initialize(PlayerMoveHandler playerMoveHandler, PlayerChildRegistry playerChildRegistry,
        GameObservables gameObservables, PlayerFacade playerFacade, SignalBus signalBus)
    {
        _playerMoveHandler = playerMoveHandler;
        _playerChildRegistry = playerChildRegistry;
        _gameObservables = gameObservables;
        _playerFacade = playerFacade;
        _signalBus = signalBus;
    }

    private bool _canMoveHorizontal = true;

    private void Awake()
    {
        _signalBus.Subscribe<SignalPlayerCountUpdate>(x => UpdateClampPoints());
        _gameObservables.GameState.Subscribe(OnGameStateChanged);
        UpdateClampPoints();
    }


    private void OnGameStateChanged(GameStates state)
    {
        if (state == GameStates.FAIL || state == GameStates.FINISH || state == GameStates.STAIR)
        {
            _canMoveHorizontal = false;
            _playerFacade.transform.ChangeLocalPosition(x: 0);
        }
    }

    private void Update()
    {
        _playerMoveHandler.MoveVertical();

        if (_canMoveHorizontal)
            _playerMoveHandler.MoveHorizontal();
    }


    private void UpdateClampPoints()
    {
        float? minChildX = null;
        float? childMaxX = null;
        foreach (var playerChild in _playerChildRegistry.ChildList)
        {
            if (playerChild.transform.localPosition.x < minChildX || minChildX == null)
                minChildX = playerChild.transform.localPosition.x;

            if (playerChild.transform.localPosition.x > childMaxX || childMaxX == null)
                childMaxX = playerChild.transform.localPosition.x;
        }

        var minX = -5.5f - minChildX;
        var maxX = 5.5f - childMaxX;
        if (minX != null)
        {
            _playerMoveHandler.ClampPositions = new Vector2((float)minX, (float)maxX);
        }
    }
}