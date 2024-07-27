using UnityEngine;
using System.Collections;

public class BaseAchievement : MonoBehaviour
{
    public AchievementType type;
    public int progress;

    public virtual void Init() { }

    public virtual void SetProgressToDefault()
    {
        if (GameDataNEW.playerAchievements.ContainsKey(type))
        {
            progress = GameDataNEW.playerAchievements[type].progress;
        }
        else
        {
            progress = 0;
        }
    }

    public virtual void Save()
    {
        if (GameDataNEW.playerAchievements.ContainsKey(type))
        {
            GameDataNEW.playerAchievements[type].progress = progress;
        }
        else
        {
            GameDataNEW.playerAchievements.Add(type, new PlayerAchievementData(type, progress));
        }
    }

    public virtual bool IsAlreadyCompleted()
    {
        return GameDataNEW.playerAchievements.IsAlreadyCompleted(type);
    }

    protected virtual void IncreaseProgress()
    {
        progress++;
    }
}
