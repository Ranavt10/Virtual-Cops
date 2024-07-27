using UnityEngine;
using Photon.Pun;

namespace TPSShooter
{
  [RequireComponent(typeof(PlayerBehaviour))]
  public class PlayerMenuWeapon : MonoBehaviourPunCallbacks
  {
    private void Start()
        {
           
                PlayerBehaviour.SelectMenuWeaponUtil util = new PlayerBehaviour.SelectMenuWeaponUtil(GetComponent<PlayerBehaviour>());
                util.SelectMenuWeapon();
            
     
    }
  }
}
