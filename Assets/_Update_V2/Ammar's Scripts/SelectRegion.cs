using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectRegion : MonoBehaviour
{
    public Image[] yellowProminency;
    public static string selectedRegion = "selectedRegion";
    private int selectedRegionNum;

    private NetworkManager network;

    private void OnEnable()
    {
        selectedRegionNum = PlayerPrefs.GetInt(selectedRegion);

        setSelectedRegion(selectedRegionNum);
    }
    // Start is called before the first frame update
    void Start()
    {
        network = FindObjectOfType<NetworkManager>();
    }

    public void setSelectedRegion(int no)
    {
        for(int i = 0; i < yellowProminency.Length; i++)
        {
            yellowProminency[i].enabled = false;
        }

        yellowProminency[no].enabled = true;

        PlayerPrefs.SetInt(selectedRegion, no);

        selectedRegionNum = no;
    }

    public void setRegion(string region)
    {
        network.ConnectToRegion(region);

        PlayerPrefs.SetString(network.regionSelected, region);

        network.region = region;
    }
}
