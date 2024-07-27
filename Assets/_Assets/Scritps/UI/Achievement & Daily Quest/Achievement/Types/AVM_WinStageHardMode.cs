using UnityEngine;
using System.Collections;

public class AVM_WinStageHardMode : BaseAchievement
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.GameEnd, (sender, param) =>
        {
            if ((bool)param && GameDataNEW.mode == GameMode.Campaign && GameDataNEW.currentStage.difficulty == Difficulty.Hard)
            {
                IncreaseProgress();
                Save();
            }
        });
    }
}
