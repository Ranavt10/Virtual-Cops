using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponPanel : MonoBehaviour
{
    public static string lastPanelSelected = "lastPanelSelected";
    public Button[] gunCategorySelectionBtn;
    public Sprite[] gunSelectionSprites;
    public Sprite[] gunUnSelectionSprites;
    public Image[] SidePanelSelectionImages;
    public GameObject[] GunPanels;
    public Image DamageFiller;
    public Image DistanceFiller;
    private int lastSelectedPanelNo = 0;
    [SerializeField]
    private Button closeBtn;
    
    // Start is called before the first frame update
    private void Start()
    {
        closeBtn.onClick.AddListener(() => setClosePanel());
    }

    private void OnEnable()
    {
        for(int i=0; i < LobbyManager.Instance.storeGunPanel.PistolWeaponSystemHandler.Length; i++)
        {
            LobbyManager.Instance.storeGunPanel.PistolWeaponSystemHandler[i].Gun.SetActive(false);
        }
        LobbyManager.Instance.Guns_Holder.SetActive(true);
        lastSelectedPanelNo = PlayerPrefs.GetInt(lastPanelSelected);
        setPanel(lastSelectedPanelNo);
    }

    public void setPanel(int no)
    {
        for (int i = 0; i < LobbyManager.Instance.storeGunPanel.PistolWeaponSystemHandler.Length; i++)
        {
            LobbyManager.Instance.storeGunPanel.PistolWeaponSystemHandler[i].Gun.SetActive(false);
        }
        for (int i = 0; i < GunPanels.Length; i++)
        {
            GunPanels[i].SetActive(false);
        }

        for(int i = 0; i < SidePanelSelectionImages.Length; i++)
        {
            SidePanelSelectionImages[i].sprite = gunUnSelectionSprites[i];
        }

        SidePanelSelectionImages[no].sprite = gunSelectionSprites[no];

        GunPanels[no].SetActive(true);

        PlayerPrefs.SetInt(lastPanelSelected, no);
        lastSelectedPanelNo = no;
    }

    private void setClosePanel()
    {
        this.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        if(LobbyManager.Instance)
            LobbyManager.Instance.Guns_Holder.SetActive(false);
    }
}

