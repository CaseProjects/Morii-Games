using Core.SignalBus;
using Events;
using UnityEngine;

public class GameMonoInstaller : MonoBehaviour
{
    [SerializeField] private GameSettingsInstaller _gameSettingsInstaller;
    [SerializeField] private PlayerChildPool _playerChildPool;
    [SerializeField] private EnemyPool _enemyPool;
    private SignalBus _signalBus;

    private void Awake()
    {
        InstallGameSettings();
        InstallSignals();
        InstallPools();
        ServiceLocator.Register(new GameObservables());
        var _ = new LevelSaveManager(_signalBus);
    }

    private void InstallGameSettings()
    {
        ServiceLocator.Register(_gameSettingsInstaller.PlayerSettings);
    }

    private void InstallSignals()
    {
        _signalBus = new SignalBus();
        ServiceLocator.Register(_signalBus);
        _signalBus.DeclareSignal<SignalSpawnPlayer>();
        _signalBus.DeclareSignal<SignalPlayerCountUpdate>();
        _signalBus.DeclareSignal<SignalStairFloorHit>();
        _signalBus.DeclareSignal<SignalUpdateCameraFollow>();
        _signalBus.DeclareSignal<SignalSaveLevel>();
    }

    private void InstallPools()
    {
        ServiceLocator.Register(_playerChildPool);
        ServiceLocator.Register(_enemyPool);
    }
}