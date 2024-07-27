using UnityEngine;
using System.Collections;

public class AVM_GetRankLevel : BaseAchievement
{
    public override void Init()
    {
        base.Init();
    }

    public override void SetProgressToDefault()
    {
        int level = GameDataNEW.playerProfile.level;

        if (GameDataNEW.playerAchievements.ContainsKey(type))
        {
            GameDataNEW.playerAchievements[type].progress = level;
        }
        else
        {
            GameDataNEW.playerAchievements.Add(type, new PlayerAchievementData(type, level, 0));
        }

        progress = GameDataNEW.playerAchievements[type].progress;
    }
}