using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TPSShooter;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSetting : MonoBehaviourPunCallbacks
{
    //PlayerBehaviour player;
    //PlayerMenuWeapon playerWeapon;
   // PlayerGrenadeProjectile playergrenade;
   // PlayerAutoshoot playershoot;
   // EnemyRadarableObject redarscript;
    //Playerhealth otherplayerscript;
    public GameObject playercamera;
    public GameObject playercanvas;
    public GameObject RPCCanvas;
    public GameObject playerMark;

    void Awake()
    {
        //MobileFPSGameManager.instance.playerCanvas = playercanvas;
        //enemy = GetComponent<EnemyBehaviour>();
        //enemyhealth=GetComponent<EnemyHealthBar>();
      ///  player = GetComponent<PlayerBehaviour>();
       // playerWeapon = GetComponent<PlayerMenuWeapon>();
       // playergrenade = GetComponent<PlayerGrenadeProjectile>();
      //  playershoot = GetComponent<PlayerAutoshoot>();
       // redarscript = GetComponent<EnemyRadarableObject>();
       // otherplayerscript = GetComponent<Playerhealth>();
       // playercanvas = GameObject.Find("PlayerHealthBar");

        if (photonView.IsMine)
        {
            
           // redarscript.enabled = false;
            //otherplayerscript.enabled = false;
            
            playercamera.SetActive(true);
            playercanvas.SetActive(true);
            RPCCanvas.SetActive(false);
            playerMark.SetActive(true);
            MobileFPSGameManager.instance.playerCanvas = playercanvas;
            MobileFPSGameManager.instance.uiHandler.RemoveAllListeners();
            MobileFPSGameManager.instance.uiHandler.AddListener(()=>PlayerCanvasSet(true));
            MobileFPSGameManager.instance.uiHandler1.RemoveAllListeners();
            MobileFPSGameManager.instance.uiHandler1.AddListener(() => PlayerCanvasSet(false));
            //  player.enabled = true;
            //  playerWeapon.enabled = true;
            //  playergrenade.enabled = true;
            //  playershoot.enabled = true;
        }
        else
        {
            //redarscript.enabled = true;
           // otherplayerscript.enabled = true;
            playercamera.SetActive(false);
            playercanvas.SetActive(false);
            RPCCanvas.SetActive(false);
            playerMark.SetActive(false);
            // player.enabled = false;
            //  playerWeapon.enabled = false;
            //  playergrenade.enabled = false;
            //  playershoot.enabled = false;
        }

    }

    void PlayerCanvasSet(bool isActive)
    {
        if (photonView.IsMine)
        {
            MobileFPSGameManager.instance.playerCanvas.SetActive(isActive);
        }
       
    }

   
}
