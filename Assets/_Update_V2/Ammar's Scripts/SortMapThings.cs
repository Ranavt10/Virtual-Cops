using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortMapThings : MonoBehaviour
{
    public int index;
    public Transform[] playerMapPositions;
    private int maxPoints;
    private HashSet<int> usedRandomPoints = new HashSet<int>();
    // Start is called before the first frame update
    void Awake()
    {
        MobileFPSGameManager.instance.PlayerPositionToSpawn[index].EnvironmentSetup = gameObject;
        for (int i = 0; i < playerMapPositions.Length; i++)
        {
            MobileFPSGameManager.instance.PlayerPositionToSpawn[index].playerPositionAccordingtoMap[i] = playerMapPositions[i].gameObject;
        }
        //MobileFPSGameManager.instance.PlayerPositionToSpawn[0].EnvironmentSetup.SetActive(true);
        //mesh.materials[0] = MobileFPSGameManager.instance.mat;
        if (MobileFPSGameManager.instance.playerPrefab != null)
        {
            for (int i = 0; i < MobileFPSGameManager.instance.PlayerPositionToSpawn.Length; i++)
            {
                if (MobileFPSGameManager.instance.PlayerPositionToSpawn[i].EnvironmentSetup != null)
                    MobileFPSGameManager.instance.PlayerPositionToSpawn[i].EnvironmentSetup.SetActive(false);
            }
            Debug.Log("Max Players in room are:" + PhotonNetwork.CurrentRoom.MaxPlayers);

            if (PhotonNetwork.CurrentRoom.MaxPlayers == 5)
            {
                MobileFPSGameManager.instance.PlayerPositionToSpawn[0].EnvironmentSetup.SetActive(true);
                MobileFPSGameManager.instance.setInputCanvas();
                maxPoints = MobileFPSGameManager.instance.PlayerPositionToSpawn[0].playerPositionAccordingtoMap.Length;
            }
            else if (PhotonNetwork.CurrentRoom.MaxPlayers == 10)
            {
                MobileFPSGameManager.instance.PlayerPositionToSpawn[1].EnvironmentSetup.SetActive(true);
                MobileFPSGameManager.instance.setInputCanvas();
                maxPoints = MobileFPSGameManager.instance.PlayerPositionToSpawn[1].playerPositionAccordingtoMap.Length;
            }
            else
            {
                MobileFPSGameManager.instance.PlayerPositionToSpawn[2].EnvironmentSetup.SetActive(true);
                MobileFPSGameManager.instance.setInputCanvas();
                maxPoints = MobileFPSGameManager.instance.PlayerPositionToSpawn[2].playerPositionAccordingtoMap.Length;
            }
        }
    }

    private void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            if (MobileFPSGameManager.instance.playerPrefab != null)
            {
                /*for(int i = 0; i < PlayerPositionToSpawn.Length; i++)
                {
                    PlayerPositionToSpawn[i].EnvironmentSetup.SetActive(false);
                }*/
                Debug.Log("Max Players in room are:" + PhotonNetwork.CurrentRoom.MaxPlayers);

                if (PhotonNetwork.CurrentRoom.MaxPlayers == 5)
                {
                    /*PlayerPositionToSpawn[0].EnvironmentSetup.SetActive(true);*/
                    //int randomPoint = Random.Range(0, playerpos.Length);
                    int randomPoint = MobileFPSGameManager.instance.spawnPositionIndex;//GetUniqueRandomPoint();
                    //PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(randomPoint, 0f, randomPoint), Quaternion.identity);
                    MobileFPSGameManager.instance.Player = PhotonNetwork.Instantiate(MobileFPSGameManager.instance.playerPrefab[MobileFPSGameManager.instance.selectedplayer].name, MobileFPSGameManager.instance.PlayerPositionToSpawn[0].playerPositionAccordingtoMap[randomPoint].transform.position, Quaternion.identity);
                    MobileFPSGameManager.instance.mobileInputCanvas.SetActive(true);
                }
                else if (PhotonNetwork.CurrentRoom.MaxPlayers == 10)
                {
                    //PlayerPositionToSpawn[1].EnvironmentSetup.SetActive(true);
                    int randomPoint = MobileFPSGameManager.instance.spawnPositionIndex;//UnityEngine.Random.Range(0, MobileFPSGameManager.instance.PlayerPositionToSpawn[1].playerPositionAccordingtoMap.Length);
                    //PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(randomPoint, 0f, randomPoint), Quaternion.identity);
                    MobileFPSGameManager.instance.Player = PhotonNetwork.Instantiate(MobileFPSGameManager.instance.playerPrefab[MobileFPSGameManager.instance.selectedplayer].name, MobileFPSGameManager.instance.PlayerPositionToSpawn[1].playerPositionAccordingtoMap[randomPoint].transform.position, Quaternion.identity);
                    MobileFPSGameManager.instance.mobileInputCanvas.SetActive(true);
                }
                else
                {
                    //PlayerPositionToSpawn[2].EnvironmentSetup.SetActive(true);
                    int randomPoint = MobileFPSGameManager.instance.spawnPositionIndex;//UnityEngine.Random.Range(0, MobileFPSGameManager.instance.PlayerPositionToSpawn[2].playerPositionAccordingtoMap.Length);
                    //PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(randomPoint, 0f, randomPoint), Quaternion.identity);
                    MobileFPSGameManager.instance.Player = PhotonNetwork.Instantiate(MobileFPSGameManager.instance.playerPrefab[MobileFPSGameManager.instance.selectedplayer].name, MobileFPSGameManager.instance.PlayerPositionToSpawn[2].playerPositionAccordingtoMap[randomPoint].transform.position, Quaternion.identity);
                    MobileFPSGameManager.instance.mobileInputCanvas.SetActive(true);
                }
            }
            else
            {
                Debug.Log("Place playerPrefab!");
            }

        }
    }

    private int GetUniqueRandomPoint()
    {
        int randomPoint;
        do
        {
            randomPoint = UnityEngine.Random.Range(0, maxPoints);
        } while (usedRandomPoints.Contains(randomPoint));

        usedRandomPoints.Add(randomPoint);
        return randomPoint;
    }
}
