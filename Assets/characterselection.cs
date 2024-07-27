using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterselection : MonoBehaviour
{
    public GameObject[] players;
    public int selectedplayer=7;
         
    // Start is called before the first frame update
    void Start()
    {
        //startSelection();
    }

    public void startSelection()
    {
        selectedplayer = PlayerPrefs.GetInt("SelectedPlayer");
        for (int i = 0; i < players.Length; i++)
        {
            players[i].SetActive(false);
        }
        players[selectedplayer].SetActive(true);
    }

    public void next()
    {
        selectedplayer++;
        if (selectedplayer> players.Length-1)
        {
            selectedplayer = 0;
        }
       
        for (int i = 0; i < players.Length; i++)
        {
            players[i].SetActive(false);
        }
        print(players.Length-1);
        print(selectedplayer);
        players[selectedplayer].SetActive(true);
        PlayerPrefs.SetInt("SelectedPlayer", selectedplayer);

    }

    public void Previous()
    {
        selectedplayer--;
        if (selectedplayer<0)
        {
            selectedplayer = players.Length-1;
        }
        
        for (int i = 0; i < players.Length; i++)
        {
            players[i].SetActive(false);
        }
        print(players.Length-1);
        print(selectedplayer);
        players[selectedplayer].SetActive(true);
        PlayerPrefs.SetInt("SelectedPlayer", selectedplayer);
    }
}
