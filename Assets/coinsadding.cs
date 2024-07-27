using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinsadding : MonoBehaviour
{
    public MapChooser MapChooserScript;


    // Start is called before the first frame update
    void Start()
    {
        MapChooserScript.enabled = true;
        if(PlayerPrefs.GetInt("FirstTimeGiveValues", 0) == 0)
        {
            PlayerPrefs.SetInt("FirstTimeGiveValues", 1);
            GameDataNEW.playerResources.ReceiveCoin(1000);
            GameDataNEW.playerResources.ReceiveGem(200);
            GameDataNEW.playerGrenades.Receive(StaticValue.GRENADE_ID_F1, 5);
        }

        GameDataNEW.playerBoosters.Receive(BoosterType.Hp, 2);
        GameDataNEW.playerBoosters.Receive(BoosterType.CoinMagnet, 2);
        GameDataNEW.playerBoosters.Receive(BoosterType.Critical, 2);
        GameDataNEW.playerBoosters.Receive(BoosterType.Damage, 2);
        GameDataNEW.playerBoosters.Receive(BoosterType.Speed, 2);
    }
}
