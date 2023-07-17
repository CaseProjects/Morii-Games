using System;
using System.Collections.Generic;
using Core.SignalBus;
using Events;
using Helpers.Extensions;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerChildRegistry
{
    private readonly PlayerFacade _playerFacade;
    private readonly PlayerChildPool _playerChildPool;
    private readonly SignalBus _signalBus;
    private readonly GameObservables _gameObservables;
    private readonly Settings _settings;

    public PlayerChildRegistry(PlayerFacade playerFacade, PlayerChildPool playerChildPool, SignalBus signalBus,
        GameObservables gameObservables, Settings settings)
    {
        _playerFacade = playerFacade;
        _playerChildPool = playerChildPool;
        _signalBus = signalBus;
        _gameObservables = gameObservables;
        _settings = settings;

        Initialize();
    }

    public readonly List<PlayerChild> ChildList = new();

    private void Initialize()
    {
        SpawnChild();
        _signalBus.Subscribe<SignalSpawnPlayer>(CalculateAndSpawnChild);
    }


    private void CalculateAndSpawnChild(SignalSpawnPlayer signalData)
    {
        var instantiateCount = signalData.StackType switch
        {
            Gate.StackType.PLUS => signalData.Value,
            Gate.StackType.MULTIPLE => ChildList.Count * signalData.Value - ChildList.Count,
        };

        for (var i = 0; i < instantiateCount; i++) SpawnChild();

        _signalBus.Fire(new SignalPlayerCountUpdate(ChildList.Count));
    }

    private void SpawnChild()
    {
        var child = _playerChildPool.Pool.Spawn();
        child.Initialize(_playerFacade.transform, this, _gameObservables);
        ChildList.Add(child);
        child.transform.parent = _settings.ChildParent;
        child.transform.SetRandomLocalPositionInSphere();
    }

    public void RemoveChild(PlayerChild playerChild)
    {
        ChildList.Remove(playerChild);
        _signalBus.Fire(new SignalPlayerCountUpdate(ChildList.Count));
        if (ChildList.Count == 0)
        {
            _gameObservables.GameState.Value = GameStates.FAIL;
        }
    }


    [Serializable]
    public class Settings
    {
        [field: SerializeField] public Transform ChildParent { get; private set; }
    }
}