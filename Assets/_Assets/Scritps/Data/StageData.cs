using UnityEngine;
using System.Collections;

public class StageData
{
    public string id;
    public Difficulty difficulty;
    public int Reward;
    public StageData(string stageNameId, Difficulty difficulty,int reward)
    {
        this.id = stageNameId;
        this.difficulty = difficulty;
        this.Reward= reward;
    }
}
