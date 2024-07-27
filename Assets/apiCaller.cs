using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;

public class apiCaller : MonoBehaviour
{
    private static readonly string baseUrl = "https://a978-72-255-51-32.ngrok-free.app/api/";
    private static readonly string GetVrcEndpoint = "getVrc";


    private static readonly string apiUrl = APIHolder.getBaseUrl() + GetVrcEndpoint;
    public Text vrctext;
    public Text withdrawabletext;

    public GameObject SelectionPanel;


    private void OnEnable()
    {
        if (PlayerPrefs.GetInt("Guest") != 1)
        {
            StartCoroutine(GetDataFromAPI());
            firebaseDB.instance.refferalcodegenerate();
        }

        SelectionPanel.SetActive(false);
    }
    

    IEnumerator GetDataFromAPI()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl))
        {

            string myHeaderValue = PlayerPrefs.GetString("Token");
            webRequest.SetRequestHeader("Authorization", myHeaderValue);

            // Request bhejna aur response ka intezar karna
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                // JSON data prapt karke use parse karna
                ApiResponseVRC response = JsonUtility.FromJson<ApiResponseVRC>(webRequest.downloadHandler.text);
                Debug.Log("Received: " + webRequest.downloadHandler.text);

                // 'vrc' value ko PlayerPrefs mein save karna
                PlayerPrefs.SetFloat("Meta", response.vrc);
                PlayerPrefs.SetFloat("gems", response.gems);
                PlayerPrefs.Save();
                vrctext.text = PlayerPrefs.GetFloat("Meta").ToString();
                withdrawabletext.text = PlayerPrefs.GetFloat("withdrawable").ToString();
                
            }
        }
    }
}
[System.Serializable]
public class ApiResponseVRC
{
    public float vrc;
    public float gems;
}


