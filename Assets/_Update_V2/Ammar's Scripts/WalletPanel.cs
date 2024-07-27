using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WalletPanel : MonoBehaviour
{
    public Button closeBtn;
    public GameObject[] panels;
    public Button[] selectionButtons;
    // Start is called before the first frame update
    void Start()
    {
        closeBtn.onClick.AddListener(() => setThePanel());

        selectionButtons[0].onClick.AddListener(() => panels[0].SetActive(true));

        selectionButtons[1].onClick.AddListener(() => panels[1].SetActive(true));
    }

    private void setThePanel()
    {
        this.gameObject.SetActive(false);
    }
}
