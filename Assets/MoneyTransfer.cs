
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using System.Collections;

public class MoneyTransfer : MonoBehaviour
{
    public static MoneyTransfer instance;
    // public Toggle rememberMe;
    public InputField walletaddress;
    public InputField amounttext;
    public Text Totalbalance;
    public Dropdown paytypetext;
    public Text messageBox;
     
    public GameObject transferbtn;
    public GameObject closebtn;
    int amount;
    string accountnumber;
    float balance;
    string signture;
    string dropvalue = "PAYID";
    private static string withdrawItem = "withdraw";
    private string withdrawapi = APIHolder.getBaseUrl() + withdrawItem;/*"https://a978-72-255-51-32.ngrok-free.app/api/withdraw";*/
  
    private IEnumerator WithdrawAmountApi(string paytype,string valetaddress, string coin)
    {
        // Creating a wwwForm object to send the parameters
        WWWForm form = new WWWForm();

        form.AddField("payType", paytype);
        form.AddField("receiverId", valetaddress);
        form.AddField("amount", coin);
       

        // Creating the UnityWebRequest and sending it
        using (UnityWebRequest www = UnityWebRequest.Post(withdrawapi, form))
        {
            // Adding a custom header with value from PlayerPrefs



            string headerValue = PlayerPrefs.GetString("Token");
            www.SetRequestHeader("Authorization", headerValue);

            // Send the request and wait for the response
            yield return www.SendWebRequest();

            // Check for errors
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                messageBox.text = www.downloadHandler.text;
                transferbtn.SetActive(true);
                closebtn.SetActive(true);

            }
            else
            {
                Debug.Log("Request sent successfully!");
                Debug.Log("Received: " + www.downloadHandler.text);
                messageBox.text = www.downloadHandler.text;
                transferbtn.SetActive(true);
                closebtn.SetActive(true);
                walletaddress.text = "";
                amounttext.text = "";
                float balance = PlayerPrefs.GetFloat("withdrawable");
                PlayerPrefs.SetFloat("withdrawable", balance - float.Parse(coin));
            }
        }
    }


    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        paytypetext.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    private void Update()
    {
        Totalbalance.text = "Total balance: " + PlayerPrefs.GetFloat("withdrawable").ToString() + " VRC";
        print(dropvalue);
        
    }
   
    private void OnDropdownValueChanged(int index)
    {
        // You can perform actions based on the selected value
        Debug.Log("Dropdown value changed to: " + paytypetext.options[index].text);
        dropvalue = paytypetext.options[index].text;

    }
    public  void trasferamount()
    {
        if (PlayerPrefs.GetInt("Guest") != 1)
        {
            StartCoroutine(WithdrawAmountApi(dropvalue,walletaddress.text, amounttext.text));
            transferbtn.SetActive(false);
            closebtn.SetActive(false);
            messageBox.text = "Please Wait For Complete Your Transation...";
        }
        else
        {
            //UnityAdsManager.instance.ShowNonRewardedAd();
            Popup.Instance.ShowToastMessage("Login For Withdraw");
        }
    }


    

   
   





   








}







