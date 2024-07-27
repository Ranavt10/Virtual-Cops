using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ChangeUsernamePanel : MonoBehaviour
{
    public static string changNameRoute = APIHolder.getBaseUrl() + "auth/updateName";
    public InputField inputField;
    // Start is called before the first frame update
    private void OnEnable()
    {
        string email = PlayerPrefs.GetString("PlayerName");
        inputField.text = email;
    }

    public void setUserName()
    {
        string changedName = inputField.text;
        StartCoroutine(changeUserName(inputField.text));
    }

    private IEnumerator changeUserName(string username)
    {
        WWWForm form = new WWWForm();

        form.AddField("username", username);

        Debug.Log("username Value:" + username);

        using (UnityWebRequest www = UnityWebRequest.Post(changNameRoute, form))
        {
            string headerValue = PlayerPrefs.GetString("Token");
            www.SetRequestHeader("Authorization", headerValue);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error Here");
                Debug.Log(www.error);
                string jsonResponse = www.downloadHandler.text;
                //Debug.Log("Received Error Response: " + jsonResponse);
                var responseObject = JsonUtility.FromJson<ChangeUserNameApi>(jsonResponse);
                LobbyManager.Instance.popUpText.text = responseObject.message;
                LobbyManager.Instance.popUpPanel.SetActive(true);
                LobbyManager.Instance.GemsEarnedSound();
                gameObject.SetActive(false);
                //Popup.Instance.ShowToastMessage(string.Format("Low Balance..."), ToastLength.Normal);
            }
            else
            {
                Debug.Log("Request sent successfully!");
                string jsonResponse = www.downloadHandler.text;
                Debug.Log("Received: " + jsonResponse);

                // Parse the JSON response
                var responseObject = JsonUtility.FromJson<ChangeUserNameApi>(jsonResponse);

                if (responseObject != null)
                {
                    PlayerPrefs.SetString("PlayerName", responseObject.username);
                    LobbyManager.Instance.userName.text = responseObject.username;
                    LobbyManager.Instance.profilePanel.GetComponent<ProfilePanel>().userNameField.text = responseObject.username;
                    inputField.text = responseObject.username;

                    PlayerPrefs.SetFloat("gems", responseObject.gems);
                    LobbyManager.Instance.FillUserData();
                    LobbyManager.Instance.popUpText.text = responseObject.message;
                    LobbyManager.Instance.popUpPanel.SetActive(true);
                    LobbyManager.Instance.GemsEarnedSound();
                    gameObject.SetActive(false);
                }
                else
                {
                    Debug.Log("Error parsing JSON response");
                }
            }
        }
    }
}

[Serializable]
public class ChangeUserNameApi
{
    public bool success;
    public string message;
    public int gems;
    public string username;
}
