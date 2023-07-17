using System;
using Core.SignalBus;
using Events;
using TMPro;

public class PlayerCountTextUpdater
{
    private readonly Settings _settings;
    private readonly SignalBus _signalBus;

    public PlayerCountTextUpdater(Settings settings, SignalBus signalBus)
    {
        _settings = settings;
        _signalBus = signalBus;

        Initialize();
    }


    private void Initialize()
    {
        _signalBus.Subscribe<SignalPlayerCountUpdate>(UpdateCountText);
    }

    private void UpdateCountText(SignalPlayerCountUpdate signalData)
    {
        _settings.CountTMP.text = signalData.PlayerCount.ToString();
    }

    [Serializable]
    public class Settings
    {
        public TextMeshPro CountTMP;
    }
}