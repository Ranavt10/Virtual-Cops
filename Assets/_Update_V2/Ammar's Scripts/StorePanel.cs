using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorePanel : MonoBehaviour
{
    public static string lastPanelSelected = "lastPanelSelectedForStore";
    public Button[] gunCategorySelectionBtn;
    public Sprite[] gunSelectionSprites;
    public Sprite[] gunUnSelectionSprites;
    public Image[] SidePanelSelectionImages;
    public GameObject[] GunPanels;

    private int lastSelectedPanelNo = 0;
    [SerializeField]
    private Button closeBtn;
    private void Start()
    {
        closeBtn.onClick.AddListener(() => setClosePanel());
    }

    private void OnEnable()
    {
        LobbyManager.Instance.setVRC();
        lastSelectedPanelNo = PlayerPrefs.GetInt(lastPanelSelected);
        setPanel(lastSelectedPanelNo);
    }

    public void setPanel(int no)
    {
        for (int i = 0; i < GunPanels.Length; i++)
        {
            GunPanels[i].SetActive(false);
        }

        for (int i = 0; i < SidePanelSelectionImages.Length; i++)
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
}
