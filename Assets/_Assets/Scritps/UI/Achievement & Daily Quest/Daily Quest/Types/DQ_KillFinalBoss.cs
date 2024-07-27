using UnityEngine;
using System.Collections;

public class DQ_KillFinalBoss : BaseDailyQuest
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.FinalBossDie, (sender, param) =>
        {
            if (GameDataNEW.mode == GameMode.Campaign)
            {
                IncreaseProgress();
            }
        });
    }
}
