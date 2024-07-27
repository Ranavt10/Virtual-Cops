﻿using UnityEngine;
using System.Collections;

public class QuestComboKillBySpecialGun : BaseQuest
{
    public int comboKillRequirement;

    private int highestComboKill;

    public override void Init()
    {
        keyDescription = StaticValue.KEY_DESCRIPTION_QUEST_COMBO_KILL_BY_SPECIAL_GUN;

        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.GetComboKillBySpecialGun, (sender, param) => SetHighestComboKill((int)param));
    }

    public override bool IsCompleted()
    {
        isCompleted = highestComboKill >= comboKillRequirement;

        return isCompleted;
    }

    public override string GetDescription()
    {
        string s = string.Format(description, comboKillRequirement);

        return s;
    }

    public override string GetCurrentProgress()
    {
        string s = string.Format("{0}/{1}", highestComboKill, comboKillRequirement);

        return s;
    }

    private void SetHighestComboKill(int count)
    {
        if (count >= highestComboKill)
        {
            highestComboKill = count;
        }
    }
}
