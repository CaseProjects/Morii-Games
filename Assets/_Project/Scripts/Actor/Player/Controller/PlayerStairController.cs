using System.Collections;
using System.Collections.Generic;
using Core.SignalBus;
using Events;
using UnityEngine;

public class PlayerStairController : MonoBehaviour
{
    private PlayerChildRegistry _playerChildRegistry;
    private PlayerFacade _playerFacade;
    private GameObservables _gameObservables;
    private SignalBus _signalBus;


    private readonly List<int> _stairGroup = new List<int>();
    private readonly List<GameObject> _stairList = new List<GameObject>();

    private bool _isBuild;

    private void Awake()
    {
        LoadServices();
        _signalBus.Subscribe<SignalStairFloorHit>(_ => OnPlayerHit());
    }

    private void LoadServices()
    {
        _playerChildRegistry = ServiceLocator.Get<PlayerChildRegistry>();
        _playerFacade = ServiceLocator.Get<PlayerFacade>();
        _gameObservables = ServiceLocator.Get<GameObservables>();
        _signalBus = ServiceLocator.Get<SignalBus>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isBuild && other.CompareTag(Constants.TAGS.PLAYER))
        {
            _isBuild = true;
            Build();
        }
    }

    private void OnPlayerHit()
    {
        var stair = _stairList[_stairList.Count - 1];
        _stairList.Remove(stair);
        if (_stairList.Count == 0)
        {
            _gameObservables.GameState.Value = GameStates.FINISH;
            return;
        }

        _signalBus.Fire(new SignalUpdateCameraFollow(stair.transform));
    }

    private void Build()
    {
        CalculateStairGroups();
        StartCoroutine(BuildStair());
    }

    private void CalculateStairGroups()
    {
        const int maxRow = 7;
        var playerChildCount = _playerChildRegistry.ChildList.Count;

        for (var i = 1; i <= maxRow; i++)
        {
            if (playerChildCount < i)
                break;

            playerChildCount -= i;
            _stairGroup.Add(i);
        }

        for (var i = maxRow; i > 0; i--)
        {
            if (playerChildCount >= i)
            {
                playerChildCount -= i;
                _stairGroup.Add(i);
                i++;
            }
        }

        _stairGroup.Sort();
    }


    private IEnumerator BuildStair()
    {
        yield return new WaitForSeconds(0.5f);

        var childCount = _playerChildRegistry.ChildList.Count;

        foreach (var inStairPlayerCount in _stairGroup)
        {
            foreach (var child in _stairList)
            {
                child.transform.localPosition += Vector3.up * 1.368f;
            }

            var playerGroup = new GameObject("PlayerGroup");
            playerGroup.transform.parent = _playerFacade.transform;
            playerGroup.transform.localPosition = Vector3.zero;

            _stairList.Add(playerGroup);

            var sumPos = Vector3.zero;
            var stairCount = 0;


            for (int i = 0; i < childCount; i++)
            {
                var child = _playerChildRegistry.ChildList[i];
                _playerChildRegistry.ChildList.Remove(child);


                child.transform.parent = playerGroup.transform;
                child.transform.localPosition = Vector3.right * (stairCount * 1f);

                sumPos += child.transform.position;

                stairCount++;
                i--;

                if (stairCount >= inStairPlayerCount)
                    break;
            }

            playerGroup.transform.position = new Vector3(-sumPos.x / inStairPlayerCount,
                playerGroup.transform.position.y,
                playerGroup.transform.position.z);

            yield return new WaitForSeconds(0.05f);
        }
    }
}