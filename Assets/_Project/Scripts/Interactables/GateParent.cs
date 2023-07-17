using UnityEngine;

public class GateParent : MonoBehaviour
{
    public bool IsGateActive { get; private set; } = true;

    public void DisableGate()
    {
        IsGateActive = false;
    }
}