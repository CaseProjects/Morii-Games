using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettingsInstaller", menuName = "Installers/GameSettingsInstaller")]
public class GameSettingsInstaller : ScriptableObject
{
    [field: SerializeField] public Player PlayerSettings { get; private set; }

    [Serializable]
    public class Player
    {
        public PlayerMoveHandler.Settings PlayerMoveHandlerSettings;
    }
}