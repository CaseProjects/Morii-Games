using System.Collections;
using System.Collections.Generic;
using Helpers.Extensions;
using TMPro;
using UnityEngine;

public class EnemyHolder : MonoBehaviour
{
    [SerializeField] private TextMeshPro _countText;
    [SerializeField] private int _enemyCount;
    private PlayerFacade _playerFacade;
    private EnemyPool _enemyPool;
    private GameObservables _gameObservables;

    private EnemyStates _enemyStates = EnemyStates.NONE;
    private readonly List<EnemyFacade> _enemyList = new List<EnemyFacade>();
    private readonly WaitForSeconds _delayYield = new WaitForSeconds(0.2f);

    private enum EnemyStates
    {
        NONE,
        SPAWNED,
        RUNNING,
    }

    private void Awake()
    {
        LoadServices();
        SetActiveCountText(false);
        StartCoroutine(CheckAreaCoroutine());
    }

    private void LoadServices()
    {
        _playerFacade = ServiceLocator.Get<PlayerFacade>();
        _enemyPool = ServiceLocator.Get<EnemyPool>();
        _gameObservables = ServiceLocator.Get<GameObservables>();
    }

    private void SetActiveCountText(bool isActive)
    {
        _countText.gameObject.SetActive(isActive);
    }

    private void Update()
    {
        MoveToPlayer();
    }

    private void MoveToPlayer()
    {
        if (_enemyStates != EnemyStates.RUNNING) return;
        foreach (var enemyFacade in _enemyList)
        {
            enemyFacade.transform.position =
                Vector3.MoveTowards(enemyFacade.transform.position, _playerFacade.transform.position,
                    Time.fixedDeltaTime * 1);
            enemyFacade.transform.LookAt(_playerFacade.transform);
        }
    }

    private IEnumerator CheckAreaCoroutine()
    {
        while (_enemyStates != EnemyStates.RUNNING)
        {
            if (_enemyStates == EnemyStates.NONE && DistanceBetweenPlayer() < 60)
            {
                _enemyStates = EnemyStates.SPAWNED;
                SetActiveCountText(true);
                InstantiateEnemies();
            }

            if (DistanceBetweenPlayer() < 10)
            {
                if (_enemyStates == EnemyStates.SPAWNED)
                {
                    _enemyStates = EnemyStates.RUNNING;
                    foreach (var enemyFacade in _enemyList) enemyFacade.PlayAnimation();
                }
            }

            yield return _delayYield;
        }
    }

    private void InstantiateEnemies()
    {
        for (int i = 0; i < _enemyCount; i++)
            SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        var enemyFacade = _enemyPool.Pool.Spawn();
        _enemyList.Add(enemyFacade);
        enemyFacade.Initialize(this);
        enemyFacade.transform.parent = transform;
        enemyFacade.transform.SetRandomLocalPositionInSphere();
    }

    public void RemoveEnemy(EnemyFacade enemyFacade)
    {
        _enemyList.Remove(enemyFacade);
        _enemyCount--;
        if (_enemyCount == 0)
        {
            gameObject.SetActive(false);
            if (_gameObservables.GameState.Value == GameStates.FIGTHING)
                _gameObservables.GameState.Value = GameStates.RUNNING;
        }

        UpdateCountText();
    }

    private void UpdateCountText()
    {
        _countText.text = _enemyCount.ToString();
    }

    private float DistanceBetweenPlayer()
    {
        return Vector3.Distance(_playerFacade.transform.position,
            transform.position);
    }

    private void OnValidate()
    {
        UpdateCountText();
    }
}