using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomPanel : MonoBehaviour
{
    public Button closeBtn;
    public Button createRoomBtn;
    public Button showRoomListBtn;
    // Start is called before the first frame update
    void Start()
    {
        closeBtn.onClick.AddListener(() => CloseThisPanel());
        createRoomBtn.onClick.AddListener(() => setCreateRoomPanelActive());
        showRoomListBtn.onClick.AddListener(() => setShowRoomPanelActive());
    }

    private void CloseThisPanel()
    {
        this.gameObject.SetActive(false);
    }

    private void setCreateRoomPanelActive()
    {
        LobbyManager.Instance.createRoomPanel.SetActive(true);
    }

    private void setShowRoomPanelActive()
    {
        
    }
}
