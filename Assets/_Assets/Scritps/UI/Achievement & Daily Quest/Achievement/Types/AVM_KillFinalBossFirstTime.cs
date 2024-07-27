using UnityEngine;
using System.Collections;

public class AVM_KillFinalBossFirstTime : BaseAchievement
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
