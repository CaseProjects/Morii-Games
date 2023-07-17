using System;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.TAGS.PLAYER_CHILD))
            if (other.TryGetComponent(out IDisposable child))
                child.Dispose();
    }
}