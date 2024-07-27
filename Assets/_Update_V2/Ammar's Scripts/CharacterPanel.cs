using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System;

public class CharacterPanel : MonoBehaviour
{
    public TextMeshProUGUI charactersNameText;
    public GameObject[] Characters;
    public TextMeshProUGUI[] CharactersPriceTexts;
    public string[] CharactersName;

    public GameObject CharactersHolder;

    public Button closeBtn;

    private int selectedPlayer = 0;

    private static string charactersList = "auth/purchaseCharacter";
    private static string buyCharacters = APIHolder.getBaseUrl() + charactersList;

    public Button purchaseBtn;

    public int HaveToPurchasedCharacter;

    private void Awake()
    {
        PlayerPrefs.SetInt("Character_" + 0, 1);
    }
    // Start is called before the first frame update

    private void Start()
    {
        closeBtn.onClick.AddListener(() => closeThisPanel());
    }

    private void OnEnable()
    {
        LobbyManager.Instance.setVRC();
        setCharacterPrices();
        if (LobbyManager.Instance)
            LobbyManager.Instance.Characters_Holder.SetActive(false);
        CharactersHolder.SetActive(true);
        selectedPlayer = PlayerPrefs.GetInt(LobbyManager.playerSelected);
        setCharacters(selectedPlayer);
    }

    private void setCharacterPrices()
    {
        Debug.Log("Length of Gun Details:" + LobbyManager.Instance.characterDetails.Count);
        for (int i = 1; i < CharactersPriceTexts.Length; i++)
        {
            CharactersPriceTexts[i].text = LobbyManager.Instance.characterDetails[i - 1].price.ToString();
        }
    }

    public void setCharacters(int no)
    {
        for (int i=0; i < Characters.Length; i++)
        {
            Characters[i].SetActive(false);

            int index = i;

            if (PlayerPrefs.GetInt("Character_" + index, 0) == 1)
            {
                CharactersPriceTexts[index].transform.parent.gameObject.SetActive(false);
            }
            else
            {
                CharactersPriceTexts[index].transform.parent.gameObject.SetActive(true);
            }
        }

        if (PlayerPrefs.GetInt("Character_" + no, 0) == 1)
        {
            purchaseBtn.gameObject.SetActive(false);
            PlayerPrefs.SetInt(LobbyManager.playerSelected, no);

            selectedPlayer = no;

            Debug.Log("Selected Character is:" + no);
        }
        else
        {
            purchaseBtn.gameObject.SetActive(true);
        }

        Characters[no].SetActive(true);
        charactersNameText.text = CharactersName[no];

        HaveToPurchasedCharacter = no;
    }

    public void PurchaseCharacter()
    {
        string purchaseItem = HaveToPurchasedCharacter.ToString();
        StartCoroutine(setCharactersOnBought(purchaseItem));
    }

    private IEnumerator setCharactersOnBought(string characterId)
    {
        WWWForm form = new WWWForm();
        form.AddField("characterId", characterId);

        Debug.Log("WeaponId Value:" + characterId);

        using (UnityWebRequest www = UnityWebRequest.Post(buyCharacters, form))
        {
            string headerValue = PlayerPrefs.GetString("Token");
            www.SetRequestHeader("Authorization", headerValue);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                string jsonResponse = www.downloadHandler.text;
                //Debug.Log("Received Error Response: " + jsonResponse);
                var responseObject = JsonUtility.FromJson<CharactersErrorResponse>(jsonResponse);
                LobbyManager.Instance.popUpText.text = responseObject.message;
                LobbyManager.Instance.popUpPanel.SetActive(true);
                //Popup.Instance.ShowToastMessage(string.Format("Low Balance..."), ToastLength.Normal);
            }
            else
            {
                Debug.Log("Request sent successfully!");
                string jsonResponse = www.downloadHandler.text;
                Debug.Log("Received: " + jsonResponse);

                // Parse the JSON response
                var responseObject = JsonUtility.FromJson<PurchaseCharacterResponse>(jsonResponse);

                if (responseObject != null)
                {
                    Debug.Log("Message: " + responseObject.message);
                    Debug.Log("Value: " + responseObject.status);
                    Debug.Log("Gems are:" + responseObject.gems);

                    PlayerPrefs.SetFloat("gems", responseObject.gems);

                    for (int i = 0; i < responseObject.characters.Length; i++)
                    {
                        string characterValue = responseObject.characters[i].ToString();
                        Debug.Log("Weapon Val is:" + characterValue);
                        string weaponPrefKey = "Character_" + characterValue; // Customize the prefix as per your need
                        PlayerPrefs.SetInt(weaponPrefKey, 1);
                    }

                    setCharacters(int.Parse(characterId));
                    LobbyManager.Instance.GemsEarnedSound();
                    LobbyManager.Instance.popUpText.text = responseObject.message;
                    LobbyManager.Instance.popUpPanel.SetActive(true);

                    LobbyManager.Instance.setVrcEconomy();
                    LobbyManager.Instance.FillUserData();
                }
                else
                {
                    Debug.Log("Error parsing JSON response");
                }
            }
        }
    }

    private void OnDisable()
    {
        if(CharactersHolder)
            CharactersHolder.SetActive(false);

        if (LobbyManager.Instance)
        {
            LobbyManager.Instance.setCharacters(selectedPlayer);
            LobbyManager.Instance.Characters_Holder.SetActive(true);
        }
    }

    private void closeThisPanel()
    {
        this.gameObject.SetActive(false);
    }
}

[Serializable]
public class PurchaseCharacterResponse
{
    public bool status;
    public string message;
    public int gems;
    public int[] characters;
}

[Serializable]
public class CharactersErrorResponse
{
    public bool status;
    public string message;
}