using UnityEngine;
using System.Collections;

public class AVM_KillFinalBossHardMode : BaseAchievement
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.FinalBossDie, (sender, param) =>
        {
            if (GameDataNEW.mode == GameMode.Campaign && GameDataNEW.currentStage.difficulty == Difficulty.Hard)
            {
                IncreaseProgress();
            }
        });
    }
}
