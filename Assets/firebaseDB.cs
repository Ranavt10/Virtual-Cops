using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using System.IO;
using UnityEditor;
using System.Collections.Generic;

public class firebaseDB : MonoBehaviour
{
    public static firebaseDB instance;
    private static string loginUser = "auth/login";
    private static string referalApi = "shareReferralCode";
    private string loginUserapi = APIHolder.getBaseUrl() + loginUser;/*"https://a978-72-255-51-32.ngrok-free.app/api/auth/login";*/
    private string shareRefferalapi = APIHolder.getBaseUrl() + referalApi;/*"https://a978-72-255-51-32.ngrok-free.app/api/shareReferralCode";*/
    //private string shareRefferalapi = "https://virtualcoop.vrcscan.com/api/shareReferralCode";

    private string localVideoPath;
    private string lobbyLocalVideoPath;
    public string dragonVideo = "dragonVideo.mp4";
    public string lobbyVideo = "lobbyVideo.mp4";

    public string dragonVideoURL = "";
    public string lobbyVideoURL = "";

    public List<string> fileIDs;
    public List<string> assetNames;

    public bool cameBackFromGamePlay;

    private static string resourceLoading = "ResourcesLoading";

    public int resourceCounter;

    public GameObject resourcesPanel;
    private void Awake()
    {
        if(PlayerPrefs.GetInt("AlreadyInstalledGame2", 0) == 0)
        {
            PlayerPrefs.DeleteAll();
            ClearPersistentDataPath();
            PlayerPrefs.SetInt("AlreadyInstalledGame2", 1);
        }
        if(instance== null)
        {
            instance = this;
        }
       
        DontDestroyOnLoad(this);

        resourceCounter = PlayerPrefs.GetInt(resourceLoading);

        if (resourceCounter < 5)
        {
            resourcesPanel.SetActive(true);
        }
        else
        {
            resourcesPanel.SetActive(false);
        }

        localVideoPath = Path.Combine(Application.persistentDataPath, dragonVideo);
        lobbyLocalVideoPath = Path.Combine(Application.persistentDataPath, lobbyVideo);
        downloadAssets();

        StartCoroutine(LoadVideo(dragonVideoURL, localVideoPath));
        StartCoroutine(LoadVideo(lobbyVideoURL, lobbyLocalVideoPath));
    }

    public void refferalcodegenerate()
    {
        StartCoroutine(GetRefferalcode());
    }

    private IEnumerator GetRefferalcode()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(shareRefferalapi))
        {
            
            string myHeaderValue = PlayerPrefs.GetString("Token");
            webRequest.SetRequestHeader("Authorization", myHeaderValue);
            yield return webRequest.SendWebRequest();
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                Debug.Log("Received: " + webRequest.downloadHandler.text);
                var json = JObject.Parse(webRequest.downloadHandler.text);
                string refferalcode = json["ReferrerCode"].ToString();
                PlayerPrefs.SetString("Refferal", refferalcode);
                print("kkkkkkkk "+ refferalcode);
            }
        }
    }
    public async void jamilShareReferralLink()
    {
        StartCoroutine(GetRefferalcode());

        string link= "https://play.google.com/store/apps/details?id=com.Virtualtech.VirtualCope";

        new NativeShare().SetSubject("Download App and Use my Reffrial Code and Get Reward, my Reffrial Code is "+ PlayerPrefs.GetString("Code")+ "Put into Input field").SetText("Virtual Cope").SetText(link +  " Download and Use My Refferal Code, My Code is " +"' " + PlayerPrefs.GetString("Refferal") +" ' "+ " Enter Into RefferalCode")
                    .SetCallback((result, shareTarget) =>
                    {
                        Debug.Log("Share result: " + result + ", selected app: " + shareTarget);
                    }
                            
                    ).Share();

    }
    private IEnumerator loginUserCallapi(string email, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);
        using (UnityWebRequest www = UnityWebRequest.Post(loginUserapi, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Request sent successfully!");
                Debug.Log("Received: " + www.downloadHandler.text);
                var json = JObject.Parse(www.downloadHandler.text);
                string token = json["token"].ToString();
                string message = json["message"].ToString();                                                   
                float vrc = float.Parse(json["vrc"].ToString());
                float gems = float.Parse(json["gems"].ToString());
                string username = json["username"].ToString();
                string userid = json["userId"].ToString();
                string useremail = json["email"].ToString();
                string walletAddress = json["walletAddress"].ToString();

                Debug.Log("Wallet Address is:" + walletAddress);

                Debug.Log("Gems are:" + gems);

                PlayerPrefs.SetString("WalletAddress", walletAddress);
                PlayerPrefs.SetString("Username", email);
                PlayerPrefs.SetString("Password", password);  
                PlayerPrefs.SetString("Token", token);
                PlayerPrefs.SetFloat("Meta", vrc);
                PlayerPrefs.SetFloat("gems", gems);
                PlayerPrefs.SetInt("Guest", 0);
                PlayerPrefs.SetInt("firsttime", 1);
                PlayerPrefs.SetString("PlayerName", username);
                PlayerPrefs.SetString("UserId", userid);
                PlayerPrefs.SetString("userEmail", useremail);

                // Retrieve and store weapons array
                JArray weaponsArray = (JArray)json["weapons"];

                // Iterate through weapons array and store values
                for (int i = 0; i < weaponsArray.Count; i++)
                {
                    // Convert the weapon value to a string
                    string weaponValue = weaponsArray[i].ToString();
                    Debug.Log("Weapon Val is:" + weaponValue);
                    string weaponPrefKey = "Weapon_" + weaponValue; // Customize the prefix as per your need
                    PlayerPrefs.SetInt(weaponPrefKey, 1);
                }

                JArray charactersArray = (JArray)json["characters"];

                for(int i=0; i< charactersArray.Count; i++)
                {
                    string characterValue = charactersArray[i].ToString();
                    Debug.Log("Character Val is:" + characterValue);
                    string characterPrefKey = "Character_" + characterValue;
                    PlayerPrefs.SetInt(characterPrefKey, 1);
                }

                JArray avatarsArray = (JArray)json["avatars"];

                for (int i = 0; i < avatarsArray.Count; i++)
                {
                    string avatarValue = avatarsArray[i].ToString();
                    Debug.Log("Avatar Val is:" + avatarValue);
                    string avatarPrefKey = "Avatar_" + avatarValue;
                    PlayerPrefs.SetInt(avatarPrefKey, 1);
                }

                JArray BannersArray = (JArray)json["banners"];

                for (int i = 0; i < BannersArray.Count; i++)
                {
                    string BannerValue = BannersArray[i].ToString();
                    Debug.Log("Banner Val is:" + BannerValue);
                    string BannerPrefKey = "Banner_" + BannerValue;
                    PlayerPrefs.SetInt(BannerPrefKey, 1);
                }

                StartCoroutine(GetRefferalcode());
                SceneFading.Instance.FadeOutAndLoadScene(StaticValue.SCENE_MENU);
            }
        }
    }
    public void login()
    {
        string Username = PlayerPrefs.GetString("Username");
        string password = PlayerPrefs.GetString("Password");
        Debug.Log("Username is:" + Username);
        StartCoroutine(loginUserCallapi(Username, password));

    }

    IEnumerator LoadVideo(string videoUrl, string localVideoPath)
    {
        if (File.Exists(localVideoPath))
        {
            // Load the video from local storage
            Debug.Log("File Exists");
        }
        else
        {
            // Download the video
            using (UnityWebRequest www = UnityWebRequest.Get(videoUrl))
            {
                yield return www.SendWebRequest();
                if (www.result == UnityWebRequest.Result.Success)
                {
                    File.WriteAllBytes(localVideoPath, www.downloadHandler.data);

                    Debug.Log("Video Loaded");
                    resourceCounter++;
                    PlayerPrefs.SetInt(resourceLoading, resourceCounter);
                }
                else
                {
                    Debug.LogError("Video download failed: " + www.error);
                }
            }
        }
    }

    public List<string> localFilePathAtRunTime;

    private void downloadAssets()
    {
        for (int i = 0; i < fileIDs.Count; i++)
        {
            string fileID = fileIDs[i];
            string assetName = assetNames[i];
            string downloadUrl = $"https://virtualcoop713.vrcscan.com/" + fileID;
            string localFilePath = Path.Combine(Application.persistentDataPath, $"downloadedAsset_{i}.unity3d");

            //if(!localFilePathAtRunTime[i].Contains(localFilePath))
                localFilePathAtRunTime.Add(localFilePath);

            if (File.Exists(localFilePath))
            {
                Debug.Log("File already exists locally: " + localFilePath);
                // Load the existing asset bundle
                //StartCoroutine(LoadGameObject(localFilePath, assetName));
            }
            else
            {
                // Start the download and loading process for each file
                StartCoroutine(DownloadFile(downloadUrl, localFilePath, assetName));
            }
        }
    }

    IEnumerator DownloadFile(string url, string localPath, string assetName)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                // Save downloaded file to local path
                File.WriteAllBytes(localPath, webRequest.downloadHandler.data);
                Debug.Log("File downloaded and saved to " + localPath);
                Debug.Log("Loaded Successfully");
                resourceCounter++;
                PlayerPrefs.SetInt(resourceLoading, resourceCounter);
                /*// Load the downloaded asset bundle
                StartCoroutine(LoadGameObject(localPath, assetName));*/
            }
        }
    }

    public void startInstantiating(int index)
    {
        StartCoroutine(LoadGameObject(localFilePathAtRunTime[index], assetNames[index]));
    }

    IEnumerator LoadGameObject(string filePath, string assetName)
    {
        AssetBundleCreateRequest bundleRequest = AssetBundle.LoadFromFileAsync(filePath);
        yield return bundleRequest;

        AssetBundle assetBundle = bundleRequest.assetBundle;

        if (assetBundle == null)
        {
            Debug.LogError("Failed to load AssetBundle!");
            yield break;
        }

        // Load the GameObject from the AssetBundle
        AssetBundleRequest assetRequest = assetBundle.LoadAssetAsync<GameObject>(assetName);
        yield return assetRequest;

        GameObject downloadedObject = assetRequest.asset as GameObject;
        if (downloadedObject != null)
        {
            Instantiate(downloadedObject);
            Debug.Log("GameObject instantiated from downloaded asset bundle: " + assetName);
        }
        else
        {
            Debug.LogError("Failed to load GameObject from AssetBundle!");
        }

        // Unload the asset bundle to free up memory
        assetBundle.Unload(false);
    }

    private void ClearPersistentDataPath()
    {
        System.IO.DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath);

        foreach (FileInfo file in di.GetFiles())
            file.Delete();
        foreach (DirectoryInfo dir in di.GetDirectories())
            dir.Delete(true);
    }
}

/*public class DeleteJSONFilesEditor
{
    [MenuItem("Tools/Delete JSON Files")]
    private static void DeleteJSONFilesMenuItem()
    {
        // Path to the directory where JSON files are stored
        string directoryPath = Application.persistentDataPath; // Or specify your custom path here

        // Check if the directory exists
        if (System.IO.Directory.Exists(directoryPath))
        {
            // Get all JSON files in the directory
            string[] jsonFiles = System.IO.Directory.GetFiles(directoryPath, "*.json");

            // Delete each JSON file
            foreach (string filePath in jsonFiles)
            {
                System.IO.File.Delete(filePath);
                Debug.Log("Deleted file: " + filePath);
            }
            Debug.Log("Deleted all JSON files.");
        }
        else
        {
            Debug.LogWarning("Directory does not exist: " + directoryPath);
        }
    }
}*/
