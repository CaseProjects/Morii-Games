using Core.SignalBus;
using Events;
using UnityEngine;

public class Stair : MonoBehaviour
{
    private SignalBus _signalBus;

    private bool _isActivate;

    private void Awake()
    {
        _signalBus = ServiceLocator.Get<SignalBus>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!_isActivate && other.gameObject.CompareTag(Constants.TAGS.PLAYER_CHILD))
        {
            _isActivate = true;
            _signalBus.Fire(new SignalStairFloorHit());
        }
    }
}