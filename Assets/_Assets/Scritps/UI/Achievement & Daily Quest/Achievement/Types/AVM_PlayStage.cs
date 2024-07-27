using UnityEngine;
using System.Collections;

public class AVM_PlayStage : BaseAchievement
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.GameEnd, (sender, param) =>
        {
            if (GameDataNEW.mode == GameMode.Campaign)
            {
                IncreaseProgress();
                Save();
            }
        });
    }
}
