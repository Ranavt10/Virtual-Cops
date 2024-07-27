using Photon.Pun;
using UnityEngine;

namespace TPSShooter
{
  public class PlayerBullet : AbstractBullet
  {

        //private PlayerBehaviour playerBehaviour;

        protected override void OnBulletCollision(RaycastHit hit)
        {

            Debug.Log("Bullet collided with: " + hit.transform.name);
            //Debug.Log("Player is:" + playerBehaviour.name);
            //if (hit.transform.GetComponent<PlayerBehaviour>()) // Enemy damage
            //{
               
               
                  //  hit.transform.GetComponent<PlayerBehaviour>().OnBulletHitother(this);
                
               
            //}


            //photonView.RPC("CreateHitEffect", RpcTarget.All, hit.point);

            //if (hit.collider.gameObject.CompareTag("Player") && !hit.collider.gameObject.GetComponent<PhotonView>().IsMine)
            //{
            //    hit.collider.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, 1f);

            //}



        }

        /*public void setPlayer(PlayerBehaviour player)
        {
            playerBehaviour = player;
        }

        public PlayerBehaviour getPlayer()
        {
            return playerBehaviour;
        }*/
  }
}
