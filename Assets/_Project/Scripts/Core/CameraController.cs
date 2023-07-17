using Cinemachine;
using Core.SignalBus;
using Events;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    private SignalBus _signalBus;

    private void Awake()
    {
        LoadServices();
        SubscribeToSignals();
    }

    private void LoadServices()
    {
        _signalBus = ServiceLocator.Get<SignalBus>();
    }

    private void SubscribeToSignals()
    {
        _signalBus.Subscribe<SignalUpdateCameraFollow>(UpdateCamera);
    }

    private void UpdateCamera(SignalUpdateCameraFollow signalData)
    {
        _virtualCamera.Follow = signalData.FollowTransform;
    }
}