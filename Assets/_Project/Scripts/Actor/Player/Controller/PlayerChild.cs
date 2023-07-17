using System;
using Core.MemoryPool;
using Helpers.Extensions;
using UnityEngine;

public class PlayerChild : MonoBehaviour, IPoolable<IMemoryPool>, IDisposable
{
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] Animator _animator;
    private IMemoryPool _pool;
    private PlayerChildRegistry _registry;
    private GameObservables _observables;
    private Transform _parent;


    public void Initialize(Transform parent, PlayerChildRegistry registry,
        GameObservables gameObservables)
    {
        _parent = parent;
        _registry = registry;
        _observables = gameObservables;

        SubscribeToSignals();
    }

    private void SubscribeToSignals()
    {
        _observables.GameState.Subscribe(OnGameStateChange);
    }


    private void OnGameStateChange(GameStates state)
    {
        switch (state)
        {
            case GameStates.BRIDGE:
                _rigidbody.SetDrag(1);
                break;
            case GameStates.RUNNING:
                _rigidbody.SetDrag(200);
                _rigidbody.ResetVelocity();
                _animator.Play(Constants.Animations.RUN);
                break;
            case GameStates.STAIR:
                _parent = null;
                break;
        }
    }

    private void FixedUpdate()
    {
        if (_parent)
        {
            transform.position =
                Vector3.MoveTowards(transform.position, _parent.position,
                    Time.fixedDeltaTime * 2);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag(Constants.TAGS.STAIR))
        {
            transform.parent = null;
            enabled = false;
        }
    }

    #region POOL

    public void OnSpawned(IMemoryPool pool)
    {
        _pool = pool;
        _animator.enabled = true;
        _animator.Play(Constants.Animations.RUN);

    }

    public void OnDespawned()
    {
        _animator.enabled = false;
        _registry?.RemoveChild(this);
        _observables?.GameState.Unsubscribe(OnGameStateChange);

    }

    public void Dispose()
    {
        _pool.Despawn(this);
    }

    #endregion
}