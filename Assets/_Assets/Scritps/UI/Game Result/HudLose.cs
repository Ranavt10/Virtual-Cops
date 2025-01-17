﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HudLose : MonoBehaviour
{
    public Text textNotiButtonHome;
    public Button btnSelectStage;
    public Button btnHome;
    public Button btnRetry;

    public void Open()
    {
        gameObject.SetActive(true);

        SetNotification();
        ShowButtons(true);
        UIController.Instance.ActiveIngameUI(false);

        CheckTutorial();
    }

    private void CheckTutorial()
    {
        if (GameDataNEW.playerTutorials.IsCompletedStep(TutorialType.RecommendUpgradeWeapon) == false)
        {
            if (GameDataNEW.playerTutorials.IsCompletedStep(TutorialType.Weapon))
            {
                GameDataNEW.playerTutorials.SetComplete(TutorialType.RecommendUpgradeWeapon);
            }
            else
            {
                TutorialGamePlayController.Instance.ShowTutorialRecommendUpgradeWeapon();
            }
        }
        else if (GameDataNEW.playerTutorials.IsCompletedStep(TutorialType.RecommendUpgradeCharacter) == false)
        {
            if (GameDataNEW.playerTutorials.IsCompletedStep(TutorialType.Character))
            {
                GameDataNEW.playerTutorials.SetComplete(TutorialType.RecommendUpgradeCharacter);
            }
            else
            {
                TutorialGamePlayController.Instance.ShowTutorialRecommendUpgradeCharacter();
            }
        }
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SoundManager.Instance.PlaySfxClick();
        SceneFading.Instance.FadeOutAndLoadScene(StaticValue.SCENE_GAME_PLAY);
    }

    public void SelectStage()
    {
        SoundManager.Instance.PlaySfxClick();

        MainMenu.navigation = MainMenuNavigation.OpenWorldMap;
        MapChooser.navigation = WorldMapNavigation.None;
        SceneFading.Instance.FadeOutAndLoadScene(StaticValue.SCENE_MENU);
    }

    public void BackToMainMenu()
    {
        SoundManager.Instance.PlaySfxClick();
        UIController.Instance.BackToMainMenu();
    }

    public void GoUpgradeWeapon()
    {
        MainMenu.navigation = MainMenuNavigation.ShowUpgradeWeapon;
        SceneFading.Instance.FadeOutAndLoadScene(StaticValue.SCENE_MENU);

        if (GameDataNEW.isShowingTutorial)
        {
            EventDispatcher.Instance.PostEvent(EventID.SubStepGoUpgradeWeaponFromLose);
        }
    }

    public void GoUpgradeSoldier()
    {
        MainMenu.navigation = MainMenuNavigation.ShowUpgradeSoldier;
        SceneFading.Instance.FadeOutAndLoadScene(StaticValue.SCENE_MENU);

        if (GameDataNEW.isShowingTutorial)
        {
            EventDispatcher.Instance.PostEvent(EventID.SubStepGoUpgradeCharacterFromLose);
        }
    }

    public void GoUpgradeSkill()
    {
        if (GameDataNEW.playerTutorials.IsCompletedStep(TutorialType.Character))
        {
            MainMenu.navigation = MainMenuNavigation.ShowUpgradeSkill;
            SceneFading.Instance.FadeOutAndLoadScene(StaticValue.SCENE_MENU);
        }
        else
        {
            GoUpgradeSoldier();
        }
    }

    private void ShowButtons(bool isShow)
    {
        btnSelectStage.gameObject.SetActive(isShow);
        btnHome.gameObject.SetActive(isShow);
        btnRetry.gameObject.SetActive(isShow);
    }

    private void SetNotification()
    {
        int readyDailyQuest = GameDataNEW.playerDailyQuests.GetNumberReadyQuest();
        int readyAchievement = GameDataNEW.playerAchievements.GetNumberReadyAchievement();

        textNotiButtonHome.transform.parent.gameObject.SetActive(readyDailyQuest > 0 || readyAchievement > 0);
    }
}
