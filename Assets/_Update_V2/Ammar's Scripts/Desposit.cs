using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Desposit : MonoBehaviour
{
    public depositType depositType;
    public TextMeshProUGUI writtenText;
    private string copyText;
    // Start is called before the first frame update
    void OnEnable()
    {
        if(depositType == depositType.email)
        {
            string email = PlayerPrefs.GetString("Username");
            copyText = email;
            setText(email);
        }
        else if(depositType == depositType.userId)
        {
            string userId = PlayerPrefs.GetString("UserId");
            copyText = userId;
            setText(userId);
        }
        else if(depositType == depositType.walletAddress)
        {
            string walletAddress = PlayerPrefs.GetString("WalletAddress");
            copyText = walletAddress;
            setText(walletAddress);
        }
    }

    public void setText(string textString)
    {
        writtenText.text = textString;
    }

    public void copyToClipBoard()
    {
        GUIUtility.systemCopyBuffer = copyText;
    }
}

public enum depositType
{
    email,
    walletAddress,
    userId,
}
