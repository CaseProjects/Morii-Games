using System;
using Helpers.Extensions;
using UnityEngine;

public class PlayerMoveHandler
{
    private readonly Settings _settings;
    private readonly PlayerFacade _player;
    private readonly GameObservables _gameObservables;

    public PlayerMoveHandler(Settings settings, PlayerFacade player, GameObservables gameObservables)
    {
        _settings = settings;
        _player = player;
        _gameObservables = gameObservables;

        Initialize();
    }

    public Vector2 ClampPositions;

    private void Initialize()
    {
        _gameObservables.GameState.Subscribe(UpdateSpeed);
    }

    private void UpdateSpeed(GameStates gameState)
    {
        _speedMultiplier = gameState switch
        {
            GameStates.IDLE => 0,
            GameStates.RUNNING => 1,
            GameStates.FIGTHING => 0.2f,
            GameStates.BRIDGE => 0.8f,
            GameStates.STAIR => 0.6f,
            GameStates.FAIL => 0,
            GameStates.FINISH => 0,
            _ => _speedMultiplier
        };
    }

    private float _speedMultiplier;
    private float _lastMouseX;
    private float _moveFactorX;
    private float _swerveAmount;


    public void MoveHorizontal()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _lastMouseX = Input.mousePosition.x;
        }
        else if (Input.GetMouseButton(0))
        {
            CalculateMoveFactor();
            CalculateSwerve();
            MoveHorizontalInternal();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _moveFactorX = 0f;
        }
    }


    #region MOVEMENT

    private void CalculateMoveFactor()
    {
        _moveFactorX = Input.mousePosition.x - _lastMouseX;
        _lastMouseX = Input.mousePosition.x;
    }

    private void CalculateSwerve()
    {
        _swerveAmount = _settings.SwerveSpeed * _speedMultiplier * _moveFactorX * Time.deltaTime;
        _swerveAmount = Mathf.Clamp(_swerveAmount, -_settings.MaxSwerveAmount,
            +_settings.MaxSwerveAmount);
    }

    #endregion


    public void MoveVertical()
    {
        _player.transform.Translate(Vector3.forward * (_settings.ForwardSpeed * _speedMultiplier * Time.deltaTime));
    }

    private void MoveHorizontalInternal()
    {
        _player.transform.Translate(_swerveAmount, 0, 0);
        _player.transform.ClampLocal(xMin: ClampPositions.x, xMax: ClampPositions.y);
    }


    [Serializable]
    public class Settings
    {
        [field: SerializeField] public float ForwardSpeed { get; private set; }
        [field: SerializeField] public float SwerveSpeed { get; private set; }
        [field: SerializeField] public float MaxSwerveAmount { get; private set; }
    }
}