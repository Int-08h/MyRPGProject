using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GlobalSaveData
{
    public GameMode selectedMode = GameMode.None;
    public string selectedMap = "";
    public Difficulty selectedDifficulty = Difficulty.None;
    public string selectedCharacterId = "";
}
