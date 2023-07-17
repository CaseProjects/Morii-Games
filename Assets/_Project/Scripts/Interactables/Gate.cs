using Core.SignalBus;
using Events;
using TMPro;
using UnityEngine;

[SelectionBase]
public class Gate : MonoBehaviour
{
    [SerializeField] private StackType _stackType;
    [SerializeField] private int _value;
    [SerializeField] private TextMeshPro _valueText;
    [SerializeField] private GameObject _centerObject;

    private GateParent _gateParent;
    private SignalBus _signalBus;

    public enum StackType
    {
        PLUS = 1,
        MULTIPLE = 2
    }


    private void Awake()
    {
        LoadServices();
        _gateParent = GetComponentInParent<GateParent>();
    }

    private void LoadServices()
    {
        _signalBus = ServiceLocator.Get<SignalBus>();
    }

    private void OnValidate() => SetTextMeshText();

    private void SetTextMeshText()
    {        _valueText.text = _stackType == StackType.PLUS ? $"+{_value}" : $"x{_value}";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.TAGS.PLAYER) &&
            (!_gateParent || (_gateParent && _gateParent.IsGateActive)))
        {
            if (_gateParent) _gateParent.DisableGate();

            _centerObject.SetActive(false);
            _signalBus.Fire(new SignalSpawnPlayer(_stackType, _value));
        }
    }
}