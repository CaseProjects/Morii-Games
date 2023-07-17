using System;
using Core.SignalBus;
using UnityEngine;

public class PlayerMonoInstaller : MonoBehaviour
{
    [Header("References")] [SerializeField]
    private PlayerFacade _playerFacade;

    [SerializeField] PlayerCollision _playerCollision;

    [SerializeField] PlayerController _playerController;
    [Header("Settings")] [SerializeField] Settings _settings;

    private void Awake()
    {
        var playerSettings = ServiceLocator.Get<GameSettingsInstaller.Player>();
        var signalBus = ServiceLocator.Get<SignalBus>();
        var gameObservables = ServiceLocator.Get<GameObservables>();
        var childPool = ServiceLocator.Get<PlayerChildPool>();

        var playerMoveHandler =
            new PlayerMoveHandler(playerSettings.PlayerMoveHandlerSettings, _playerFacade, gameObservables);
        var playerChildRegistry = new PlayerChildRegistry(_playerFacade, childPool, signalBus, gameObservables,
            _settings.ChildRegistry);
        var playerCountText = new PlayerCountTextUpdater(_settings.CountText, signalBus);

        _playerController.Initialize(playerMoveHandler, playerChildRegistry, gameObservables, _playerFacade, signalBus);
        _playerCollision.Initialize(gameObservables);
        ServiceLocator.Register(_playerFacade);
        ServiceLocator.Register(playerChildRegistry);
    }

    [Serializable]
    private class Settings
    {
        public PlayerChildRegistry.Settings ChildRegistry;
        public PlayerCountTextUpdater.Settings CountText;
    }
}