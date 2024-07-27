using UnityEngine;
using Photon.Pun;
using LightDev;

namespace TPSShooter
{
  [RequireComponent(typeof(PlayerBehaviour))]
  public class PlayerAutoshoot : MonoBehaviourPunCallbacks
  {
    public bool isEnabled = true;
    public bool useSavedData = true;

    private PlayerBehaviour player;

    private void Awake()
    {
            if (photonView.IsMine)
            {
                player = GetComponent<PlayerBehaviour>();
                if (useSavedData)
                {
                    isEnabled = SaveLoad.IsAutoShoot;
                }
            }
            
                
              
      
    }

    private void Update()
    {
            if (photonView.IsMine)
            {
                if (IsAutoShootNeeded())
                {
                    Events.FireRequested.Call();
                }
            }

                
            
    }

    private bool IsAutoShootNeeded()
    {
           
      if (!isEnabled) return false;
      if (!player) return false;
      if (!player.FireHitObject) return false;

      return false;
    }
  }
}
