using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameManager
{
    public static GameMode selectedMode;
    public static string selectedMap;
    public static Difficulty selectedDifficulty;
    public static string selectedCharacterId;

    public static void Reset()
    {
        selectedMode = GameMode.None;
        selectedMap = null;
        selectedDifficulty = Difficulty.None;
        selectedCharacterId = null;
    }
}