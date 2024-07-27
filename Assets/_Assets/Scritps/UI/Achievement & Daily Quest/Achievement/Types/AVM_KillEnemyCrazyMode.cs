using UnityEngine;
using System.Collections;

public class AVM_KillEnemyCrazyMode : BaseAchievement
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.UnitDie, (sender, param) =>
        {
            if (GameDataNEW.mode == GameMode.Campaign && GameDataNEW.currentStage.difficulty == Difficulty.Crazy)
            {
                IncreaseProgress();
            }
        });
    }
}
