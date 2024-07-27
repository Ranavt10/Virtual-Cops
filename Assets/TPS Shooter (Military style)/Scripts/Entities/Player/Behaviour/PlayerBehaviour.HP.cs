using UnityEngine;
using LightDev;
using Photon.Pun;
using UnityEngine.UI;
using System.Collections.Generic;
using Photon.Realtime;
using System.Collections;

namespace TPSShooter
{
    public partial class PlayerBehaviour
    {
        private const float MaxHP = 100;
        private float hp = MaxHP;
        private float damage = 1;

        public float GetMaxHP() { return MaxHP; }
        public float GetCurrentHP() { return hp; }

        [PunRPC]
        public void OnBulletHit(PlayerBullet bullet)
        {
            //jamil change code enemy bullet to playerbullet
            sounds.PlaySound(sounds.HitSound);
            DecreaseHP(bullet.damage);
            Events.PlayerBulletHit.Call(bullet);
        }
        [PunRPC]
        public void OnBulletHitother(PlayerBullet bullet)
        {
            //if (currentState == deathState) return;


            if (!photonView.IsMine)
            {


                DecreaseHP(bullet.damage);

                if (BloodEffect != null)
                {
                    BloodEffect.transform.position = bullet.transform.position;
                    BloodEffect.Stop();
                    BloodEffect.Play();
                }

                if (hp <= 0)
                {
                    //Die();
                }
            }
        }




        #region fps code



        public void Fire1()
        {
            PlayerBullet bullet;
            bullet = GetComponent<PlayerBullet>();

            RaycastHit _hit;
            Ray ray = FPS_Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));

            if (Physics.Raycast(ray, out _hit, 100))
            {
                Debug.Log(_hit.collider.gameObject.name);

                photonView.RPC("CreateHitEffect", RpcTarget.All, _hit.point);

                if (_hit.collider.gameObject.CompareTag("Player") && !_hit.collider.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    _hit.collider.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, /*100f*/ mobileFPSGame.damage, PhotonNetwork.NickName);

                }

            }

        }
        private bool oneTime;

        [PunRPC]
        public void TakeDamage(float _damage, string killerNickname, PhotonMessageInfo info)
        {
            DecreaseHP(_damage);
            healthBar.fillAmount = hp / MaxHP;

            if (hp <= 0f)
            {
                photonView.RPC(nameof(SetKillerName), RpcTarget.Others, killerNickname, photonView.Owner.NickName);
                photonView.RPC(nameof(Die), RpcTarget.AllBuffered);
            }
        }

        [ContextMenu("Test Name")]
        public void TestKillerName()
        {
            photonView.RPC(nameof(SetKillerName), RpcTarget.All);
        }

        [PunRPC]
        public void SetKillerName(string killerName, string victimName/*PhotonMessageInfo info*/)
        {
            killedText.text = killerName + " killed " + victimName;/*info.Sender.NickName + " killed " + info.photonView.Owner.NickName;*/
            userNameKilled.SetActive(true);
            CancelInvoke(nameof(SetKilledPanel));
            Invoke(nameof(SetKilledPanel), 10f);
        }

        public void SetKilledPanel()
        {
            userNameKilled.SetActive(false);
        }

        [PunRPC]
        public void CreateHitEffect(Vector3 position)
        {


            BloodEffect.transform.position = position;
            BloodEffect.Stop();
            BloodEffect.Play();


        }

        IEnumerator Die1()
        {
            yield return new WaitForSeconds(1f);

            Respawn();
        }

        void Respawn()
        {
            if (PhotonNetwork.CurrentRoom.MaxPlayers == 5)
            {
                /*PlayerPositionToSpawn[0].EnvironmentSetup.SetActive(true);*/
                //int randomPoint = Random.Range(0, playerpos.Length);
                int randomPoint = Random.Range(0, MobileFPSGameManager.instance.PlayerPositionToSpawn[0].playerPositionAccordingtoMap.Length);
                //PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(randomPoint, 0f, randomPoint), Quaternion.identity);
                //PhotonNetwork.Instantiate(playerPrefab[selectedplayer].name, playerpos[randomPoint].transform.position, Quaternion.identity);
                transform.position = new Vector3(MobileFPSGameManager.instance.PlayerPositionToSpawn[0].playerPositionAccordingtoMap[randomPoint].transform.position.x, MobileFPSGameManager.instance.PlayerPositionToSpawn[0].playerPositionAccordingtoMap[randomPoint].transform.position.y, MobileFPSGameManager.instance.PlayerPositionToSpawn[0].playerPositionAccordingtoMap[randomPoint].transform.position.z);
            }
            else if (PhotonNetwork.CurrentRoom.MaxPlayers == 10)
            {
                //PlayerPositionToSpawn[1].EnvironmentSetup.SetActive(true);
                int randomPoint = Random.Range(0, MobileFPSGameManager.instance.PlayerPositionToSpawn[1].playerPositionAccordingtoMap.Length);
                //PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(randomPoint, 0f, randomPoint), Quaternion.identity);
                //PhotonNetwork.Instantiate(playerPrefab[selectedplayer].name, PlayerPositionToSpawn[1].playerPositionAccordingtoMap[randomPoint].transform.position, Quaternion.identity);
                transform.position = new Vector3(MobileFPSGameManager.instance.PlayerPositionToSpawn[1].playerPositionAccordingtoMap[randomPoint].transform.position.x, MobileFPSGameManager.instance.PlayerPositionToSpawn[1].playerPositionAccordingtoMap[randomPoint].transform.position.y, MobileFPSGameManager.instance.PlayerPositionToSpawn[1].playerPositionAccordingtoMap[randomPoint].transform.position.z);
            }
            else
            {
                //PlayerPositionToSpawn[2].EnvironmentSetup.SetActive(true);
                int randomPoint = Random.Range(0, MobileFPSGameManager.instance.PlayerPositionToSpawn[2].playerPositionAccordingtoMap.Length);
                //PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(randomPoint, 0f, randomPoint), Quaternion.identity);
                //PhotonNetwork.Instantiate(playerPrefab[selectedplayer].name, PlayerPositionToSpawn[2].playerPositionAccordingtoMap[randomPoint].transform.position, Quaternion.identity);
                transform.position = new Vector3(MobileFPSGameManager.instance.PlayerPositionToSpawn[2].playerPositionAccordingtoMap[randomPoint].transform.position.x, MobileFPSGameManager.instance.PlayerPositionToSpawn[2].playerPositionAccordingtoMap[randomPoint].transform.position.y, MobileFPSGameManager.instance.PlayerPositionToSpawn[2].playerPositionAccordingtoMap[randomPoint].transform.position.z);
            }
            /*int randomPoint = Random.Range(0, MobileFPSGameManager.instance.playerpos.Length);*/

            //transform.position = new Vector3(MobileFPSGameManager.instance.playerpos[randomPoint].transform.position.x, MobileFPSGameManager.instance.playerpos[randomPoint].transform.position.y, MobileFPSGameManager.instance.playerpos[randomPoint].transform.position.z);
            IsAlive = true;
            _animator.SetTrigger(animationsParameters.responeTrigger);
            weaponSettings.CurrentWeapon.gameObject.SetActive(true);

            _characterController.enabled = true;
            _animator.SetBool(animationsParameters.aimingBool, true);

            setSkinnedMeshRenderer(true);
            photonView.RPC("RegainHealth", RpcTarget.AllBuffered);

        }

        [PunRPC]
        public void RegainHealth()
        {
            hp = MaxHP;
            healthBar.fillAmount = hp / MaxHP;
            _animator.SetTrigger(animationsParameters.responeTrigger);

            Events.PlayerChangedHP.Call();
        }
        #endregion


        public void OnGrenadeHit(AbstractGrenade grenade)
        {
            if (Vector3.Distance(transform.position, grenade.transform.position) < 2)
            {
                //Die();
            }
        }
        [PunRPC]
        public void OnZombieHit(PlayerBehaviour zombie)
        {
            sounds.PlaySound(sounds.HitSound);
            DecreaseHP(zombie.damage);
            Events.PlayerZombieHit.Call(zombie);
        }

        [PunRPC]
        public void IncreaseHP(float deltaHP)
        {
            AddDeltaHP(deltaHP);
        }
        [PunRPC]
        public void DecreaseHP(float hp)
        {
            AddDeltaHP(-hp);
            print(hp);
        }

        private void AddDeltaHP(float deltaHP)
        {
            hp += deltaHP;
            hp = Mathf.Clamp(hp, 0, MaxHP);
            Events.PlayerChangedHP.Call();

            //if (hp <= 0)
            //  Die();
        }
    }
}
