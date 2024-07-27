﻿using UnityEngine;
using System.Collections;

public class AVM_OwnSpecialGun : BaseAchievement
{
    public override void Init()
    {
        base.Init();
    }

    public override void SetProgressToDefault()
    {
        int num = GameDataNEW.playerGuns.GetNumberOfSpecialGun();

        if (GameDataNEW.playerAchievements.ContainsKey(type))
        {
            GameDataNEW.playerAchievements[type].progress = num;
        }
        else
        {
            GameDataNEW.playerAchievements.Add(type, new PlayerAchievementData(type, num, 0));
        }

        progress = GameDataNEW.playerAchievements[type].progress;
    }

}