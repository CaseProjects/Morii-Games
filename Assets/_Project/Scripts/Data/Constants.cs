using UnityEngine;

public static class Constants
{
    public static class PlayerPrefsKey
    {
        public const string LEVEL = "CurrentLevel";
    }

    public static class Animations
    {
        public static readonly int IDLE = Animator.StringToHash("IDLE");
        public static readonly int RUN = Animator.StringToHash("RUN");
    }
    public static class TAGS
    {
        public const string PLAYER = "Player";
        public const string PLAYER_CHILD = "Player/Child";
        public const string ENEMY = "Enemy";
        public const string ENEMY_CHILD = "Enemy/Child";
        public const string OBSTACLE = "Obstacle";
        public const string BRIDGE = "Bridge";
        public const string STAIR = "Stair";
        public const string FINISH = "Finish";
        
    }
}