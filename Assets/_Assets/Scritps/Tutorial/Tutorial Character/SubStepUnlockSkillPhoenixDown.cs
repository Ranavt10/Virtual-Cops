using UnityEngine;
using System.Collections;

public class SubStepUnlockSkillPhoenixDown : TutorialSubStep
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.SubStepUnlockSkillPhoenixDown, (sender, param) =>
        {
            Next();
            GameDataNEW.playerTutorials.SetComplete(TutorialType.Character);
        });
    }
}
