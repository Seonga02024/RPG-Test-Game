public static class SceneData {
    public enum SceneType {
        None = -1,
        Main = 0
    }
    public static string GetSceneName(SceneType type) {
        switch (type) {
            case SceneType.None:
                return "None";
            case SceneType.Main:
                return "Main";
            default:
                return "None";
        }
    }
}

public static class SettingGameData {
    public const float EACH_STAGE_BASIC_MONSTER_UPGRADE_RATIO = 0.1f;
    public const int EACH_UPGRADE_CHARACTER_HP = 5;
    public const int EACH_UPGRADE_CHARACTER_ATTACK = 2;
    public const int EACH_UPGRADE_CHARACTER_HP_BASIC_COIN = 10;
    public const int EACH_UPGRADE_CHARACTER_ATTACK_BASIC_COIN = 50;
    public const float EACH_UPGRADE_CHARACTER_HP_COIN_RATIO = 2;
    public const float EACH_UPGRADE_CHARACTER_ATTACK_COIN_RATIO = 1.5f;
    public const int EACH_UPGRADE_CHARACTER_HP_NUM = 10;
    public const int EACH_UPGRADE_CHARACTER_ATTACK_NUM = 2;
}

public enum CurrentMonsterState{
    Idle =0,
    Run = 1,
    Attack = 2,
    Die = 3 
}

public enum CurrentCharacterState{
    Idle =0,
    Move = 1,
    Attack = 2,
    Die = 3
}
