using UnityEngine;

namespace Events
{

    public struct SignalSaveLevel
    {
        
    }
    public struct SignalSpawnPlayer
    {
        public readonly Gate.StackType StackType;
        public readonly int Value;

        public SignalSpawnPlayer(Gate.StackType stackType, int value)
        {
            StackType = stackType;
            Value = value;
        }
    }

    public struct SignalPlayerCountUpdate
    {
        public readonly int PlayerCount;

        public SignalPlayerCountUpdate(int playerCount)
        {
            PlayerCount = playerCount;
        }
    }


    public struct SignalStairFloorHit
    {
        
    }
    
    public struct SignalUpdateCameraFollow
    {
        public readonly Transform FollowTransform;

        public SignalUpdateCameraFollow(Transform followTransform)
        {
            FollowTransform = followTransform;
        }
    }
}