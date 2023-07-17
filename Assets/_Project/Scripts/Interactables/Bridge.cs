using DG.Tweening;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    [SerializeField] private GameObject[] _parts;

    private void Awake()
    {
        for (var i = 0; i < _parts.Length; i++)
        {
            var part = _parts[i];
            part.transform.DOLocalMoveX(5, 1 + (float)i / 6).SetEase(Ease.Linear)
                .onComplete += () =>
            {
                part.transform.DOLocalMoveX(0, 1 + (float)i / 6).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
            };
        }
    }
}