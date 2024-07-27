using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class openGiftBox : MonoBehaviour
{
    public Text UserTExt;
    public GameObject giftpenal;
    public GameObject menupenal;

    private void OnEnable()
    {
        UserTExt.text = "Congradulation Mr." + PlayerPrefs.GetString("PlayerName") + " You Win 0.2 Coin...";
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("gift") == 0)
        {
            giftpenal.SetActive(true);
            menupenal.SetActive(false);
            PlayerPrefs.SetInt("gift", 1);
        }
        else
        {
            giftpenal.SetActive(false);
            menupenal.SetActive(true);
        }
    }
    public void okbtn()
    {
        menupenal.SetActive(true);
        giftpenal.SetActive(false);
    }
}
