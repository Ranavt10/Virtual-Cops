using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    public Button logOutBtn;
    public GameObject settings;

    private int lastSelected;

    public GameObject deletePanel;
    // Start is called before the first frame update
    void Start()
    {
        logOutBtn.onClick.AddListener(() => Logout());
    }

    private void OnEnable()
    {
        lastSelected = PlayerPrefs.GetInt("SettingPanelSelected");
        Popup.Instance.setSelectedItem(lastSelected);
        deletePanel.SetActive(false);
    }

    public void Logout()
    {
        SoundManager.Instance.onClickSound();
        SceneManager.LoadScene(0);
        settings.SetActive(false);
        Popup.Instance.setSelectedItemTurnOff();
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("ResourcesLoading", 5);
        gameObject.SetActive(false);
    }

    public void clickSoundEffect()
    {
        SoundManager.Instance.onClickSound();
    }
}
