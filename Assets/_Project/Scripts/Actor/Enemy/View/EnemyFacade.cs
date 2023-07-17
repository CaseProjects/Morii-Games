using System;
using Core.MemoryPool;
using UnityEngine;

public class EnemyFacade : MonoBehaviour, IPoolable<IMemoryPool>, IDisposable
{
    [SerializeField] private Animator _animator;
    private IMemoryPool _pool;
    private EnemyHolder _enemyHolder;
    private bool _isActivate;

    public void OnSpawned(IMemoryPool pool)
    {
        _pool = pool;
    }


    public void OnDespawned()
    {
        _animator.enabled = false;
        _enemyHolder?.RemoveEnemy(this);
    }

    public void Initialize(EnemyHolder enemyHolder)
    {
        _enemyHolder = enemyHolder;
        _isActivate = true;
        _animator.enabled = true;
    }

    public void PlayAnimation()
    {
        _animator.Play(Constants.Animations.RUN);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_isActivate && other.gameObject.CompareTag(Constants.TAGS.PLAYER_CHILD))
        {
            if (other.gameObject.TryGetComponent(out PlayerChild playerChild) && other.gameObject.activeInHierarchy)
            {
                _isActivate = false;
                playerChild.Dispose();
                Dispose();
            }
        }
    }

    public void Dispose()
    {
        _pool.Despawn(this);
    }
}