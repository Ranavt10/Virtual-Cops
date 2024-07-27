using UnityEngine;
using System.Collections;

public class BaseDailyQuest : MonoBehaviour
{
    public DailyQuestType type;
    public int progress;

    public virtual void Init() { }

    public virtual void SetProgressToDefault()
    {
        for (int i = 0; i < GameDataNEW.playerDailyQuests.Count; i++)
        {
            PlayerDailyQuestData quest = GameDataNEW.playerDailyQuests[i];

            if (quest.type == type)
            {
                progress = quest.progress;
                break;
            }
        }
    }

    public virtual void Save()
    {
        for (int i = 0; i < GameDataNEW.playerDailyQuests.Count; i++)
        {
            PlayerDailyQuestData quest = GameDataNEW.playerDailyQuests[i];

            if (quest.type == type)
            {
                quest.progress = progress;
                break;
            }
        }
    }

    public virtual bool IsAlreadyClaimed()
    {
        return GameDataNEW.playerDailyQuests.IsAlreadyClaimed(type);
    }

    protected virtual void IncreaseProgress()
    {
        progress++;
    }
}
