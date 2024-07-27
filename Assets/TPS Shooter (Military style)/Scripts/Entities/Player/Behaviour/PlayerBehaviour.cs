using UnityEngine;
using Photon.Pun;
using LightDev;
using LightDev.Core;
using UnityEngine.UI;
using System.Collections;
using TMPro;

namespace TPSShooter
{
    [RequireComponent(typeof(FootstepSounds))]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Animator))]
    public partial class PlayerBehaviour : Base
    {
        [SerializeField]
        private float[] damageType;
        public PlayerWeaponSettings weaponSettings = new PlayerWeaponSettings();
        public PlayerGrenadeSettings grenadeSettings = new PlayerGrenadeSettings();
        public PlayerSounds sounds = new PlayerSounds();
        public PlayerSettingsIK IkSettings = new PlayerSettingsIK();
        public PlayerCrouchSettings crouchSettings = new PlayerCrouchSettings();
        public PlayerMovementSettings movementSettings = new PlayerMovementSettings();
        private PlayerAnimationParameters animationsParameters = new PlayerAnimationParameters();

        private CharacterController _characterController;
        private Animator _animator;

        private static PlayerBehaviour instance;
        public bool IsAlive { get; private set; } = true;
        public float Noise { get { return GetNoise(); } }
        public static PlayerBehaviour GetInstance() { return instance; }
        public ParticleSystem BloodEffect;
        public Camera FPS_Camera;
        public Image healthBar;
        int selectedGun = 0;

        public MobileFPSGameManager mobileFPSGame;

        public SkinnedMeshRenderer skinnedMeshRenderer;

        [SerializeField]
        private PunController punController;

        public int positionSecured;


        private Coroutine minimapCoroutine;

        public MiniMapComponent miniMapComponent;

        public MiniMapComponent enemyMiniMapComponent;
        public GameObject enemyDetection;

        public LayerMask layerToAssignMinimap;

        public GameSceneManager gameManagerScene;

        public GameObject userName;
        public TextMeshProUGUI userNameText;
        public GameObject userNameCanvas;

        public GameObject userNameKilled;
        public TextMeshProUGUI killedText;

        #region MonoBehaviour

        private void OnValidate()
        {

            if (photonView.IsMine)
            {
                crouchSettings.CharacterHeightCrouching = Mathf.Clamp(crouchSettings.CharacterHeightCrouching, 0, GetComponent<CharacterController>().height);
                crouchSettings.CharacterCenterCrouching.y = Mathf.Max(crouchSettings.CharacterCenterCrouching.y, 0);
                movementSettings.AirSpeed = Mathf.Max(movementSettings.AirSpeed, 0);
                movementSettings.JumpSpeed = Mathf.Max(movementSettings.JumpSpeed, 0);
                movementSettings.JumpTime = Mathf.Max(movementSettings.JumpTime, 0);
                movementSettings.ForwardSpeed = Mathf.Max(movementSettings.ForwardSpeed, 0);
                movementSettings.StrafeSpeed = Mathf.Max(movementSettings.StrafeSpeed, 0);
                movementSettings.SprintSpeed = Mathf.Max(movementSettings.SprintSpeed, 0);
                movementSettings.CrouchForwardSpeed = Mathf.Max(movementSettings.CrouchForwardSpeed, 0);
                movementSettings.CrouchStrafeSpeed = Mathf.Max(movementSettings.CrouchStrafeSpeed, 0);
                ValidateWeapons();
            }
        }

        private void Awake()
        {
            if (photonView.IsMine)
            {
                instance = this;
                gameManagerScene = FindObjectOfType<GameSceneManager>();
                selectedGun = PlayerPrefs.GetInt("StoreGuns");
                photonView.RPC(nameof(setGuns), RpcTarget.AllBuffered, selectedGun);
                MiniMapManager.Instance.mapController.target = gameObject.transform;
                miniMapComponent.enabled = true;
                enemyMiniMapComponent.enabled = false;
                enemyDetection.SetActive(false);
                _characterController = GetComponent<CharacterController>();
                _animator = GetComponent<Animator>();
                _animator.applyRootMotion = movementSettings.ApplyRootMotion;
                CheckLayers();
                InitializeWeapon();
                InitializeCrouch();
                Subscribe();

                mobileFPSGame = FindObjectOfType<MobileFPSGameManager>();
                punController = GetComponent<PunController>();

                mobileFPSGame.QuitMatchBtn.onClick.AddListener(ExitFunctionCalled);
                mobileFPSGame.exitEvent.RemoveAllListeners();
                mobileFPSGame.exitEvent.AddListener(ExitFunctionCalled);
                mobileFPSGame.damage = damageType[selectedGun];
            }
            else
            {
                enemyMiniMapComponent.enabled = false;
                enemyDetection.SetActive(false);
                miniMapComponent.enabled = false;
                //userNameKilled.SetActive(false);
            }
            
        }

        private void Start()
        {
            if (photonView.IsMine)
            {
                mobileFPSGame.YouWinPanel.GetComponent<YouWinPanel>().playerBehaviour = GetComponent<PlayerBehaviour>();
                mobileFPSGame.GameOverPanel.GetComponent<GameOverPanel>().playerBehaviour = GetComponent<PlayerBehaviour>();
                /*photonView.RPC("setUserName", RpcTarget.Others);*/
                userNameCanvas.SetActive(false);
                userName.SetActive(false);
                photonView.RPC("setUserName", RpcTarget.Others);
            }
            else
            {
                userNameCanvas.SetActive(true);
                userName.SetActive(true);
                userNameText.text = photonView.Owner.NickName;
            }
        }

        [PunRPC]
        private void setUserName()
        {
            userNameCanvas.SetActive(true);
            userName.SetActive(true);
            userNameText.text = photonView.Owner.NickName;//PlayerPrefs.GetString("PlayerName");
        }

        [PunRPC]
        private void setGuns(int index)
        {
            weaponSettings.AllWeapons[0] = weaponSettings.AllWeapons[index];
            Debug.Log("Selcted Gun is:" + index);
            weaponSettings.CurrentWeapon = weaponSettings.AllWeapons[0];
            weaponSettings.CurrentWeapon.gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            OnDestroyPositionCalled();
            Unsubscribe();
        }

        private void OnDestroyPositionCalled()
        {
            if (photonView.IsMine)
            {
                if (IsAlive)
                {
                    gameManagerScene.photonView.RPC("PlayerLeft", RpcTarget.All, photonView.ViewID);
                }
            }
        }

        private void Update()
        {
            if (photonView.IsMine)
            {
                //CheckGroundStatus();
                UpdateGroundCheck();
                UpdateWalk();
                UpdateRun();
                UpdateGravity();
                UpdateMovementSpeed();
                UpdateFirePoint();
                /*if (IsAlive)
                {
                    _characterController.enabled = true;
                }*/
            }
            else
            {
                /*if (IsAlive)
                {
                    _characterController.enabled = true;
                }*/
            }
        }

        private void LateUpdate()
        {
            if (photonView.IsMine)
            {
                UpdateSpineIK();
            }
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (photonView.IsMine)
            {
                UpdateLeftHandIK();
            }
        }

        #endregion

        private void CheckLayers()
        {
            if (!LayerMask.LayerToName(gameObject.layer).Equals(Layers.Player))
                Debug.LogError("PlayerBehaviour: Player has to be layered as Player.");
        }

        private void Subscribe()
        {
            if (photonView.IsMine)
            {
                Events.JumpRequested += OnJumpRequested;
                Events.CrouchRequested += OnCrouchRequested;
                Events.FireRequested += OnFireRequested;
                Events.ReloadRequested += OnReloadRequested;
                Events.SwapWeaponRequested += OnSwapWeaponRequested;
                Events.DropWeaponRequested += OnDropWeaponRequested;
                Events.GrenadeStartThrowRequest += OnGrenadeStartThrowRequest;
                Events.GrenadeFinishThrowRequest += OnGrenadeFinishThrowRequest;
                Events.AimActivateRequested += OnAimActivateRequested;
            }
        }

        private void Unsubscribe()
        {
            if (photonView.IsMine)
            {
                Events.JumpRequested -= OnJumpRequested;
                Events.CrouchRequested -= OnCrouchRequested;
                Events.FireRequested -= OnFireRequested;
                Events.ReloadRequested -= OnReloadRequested;
                Events.SwapWeaponRequested -= OnSwapWeaponRequested;
                Events.DropWeaponRequested -= OnDropWeaponRequested;
                Events.GrenadeStartThrowRequest -= OnGrenadeStartThrowRequest;
                Events.GrenadeFinishThrowRequest -= OnGrenadeFinishThrowRequest;
                Events.AimActivateRequested -= OnAimActivateRequested;
            }
        }
        private float GetNoise()
        {
            if (IsFire) return 30;
            if (_jumpingTriggered) return 7;
            if (_animator.GetFloat(animationsParameters.verticalMovementFloat) != 0 || _animator.GetFloat(animationsParameters.horizontalMovementFloat) != 0)
            {
                return IsCrouching ? 3 : 5;
            }
            if (IsReloading) return 3;
            return 0;
        }
        [PunRPC]
        public void Die()
        {
            // if (!IsAlive) return;
            //   Events.PlayerDied.Call();

            if (photonView.IsMine)
            {
                /*if (PhotonNetwork.IsMasterClient)
                {
                    mobileFPSGame.TransferMasterClient();
                }*/
                IsAlive = false;

                _characterController.enabled = false;

                _animator.SetTrigger(animationsParameters.dieTrigger);

                _animator.SetBool(animationsParameters.aimingBool, false);

                //if (IsThrowingGrenade)
                //    Events.GrenadeFinishThrowRequest.Call();

                if (IsAiming)
                    DeactivateAiming();
                _characterController.enabled = false;
                //_animator.SetTrigger(animationsParameters.dieTrigger);

                setSkinnedMeshRenderer(false);
                weaponSettings.CurrentWeapon.gameObject.SetActive(false);
                ReceiveRank(PhotonNetwork.CurrentRoom.PlayerCount);
                /*GameSceneManager gameManager = FindObjectOfType<GameSceneManager>();
                gameManager.photonView.RPC("PlayerKilled", RpcTarget.All, photonView.ViewID);*/
                //StartCoroutine(Die1());
                //punController.AddToGameManagerList();
            }
            else
            {
                IsAlive = true;
                _characterController.enabled = true;
                setSkinnedMeshRenderer(true);
                weaponSettings.CurrentWeapon.gameObject.SetActive(true);
            }
        }

        /*[PunRPC]*/
        public void ReceiveRank(int rank)
        {
            // Handle receiving the rank and displaying it to the player
            Debug.Log("Rank: " + rank);

            for(int i=0; i < mobileFPSGame.Canvases.Length; i++)
            {
                mobileFPSGame.Canvases[i].SetActive(false);
            }

            if (rank == 1)
            {
                // Winner
                mobileFPSGame.YouWinPanel.GetComponent<YouWinPanel>().position = rank;
                mobileFPSGame.YouWinPanel.SetActive(true);
            }
            else
            {
                // Game over
                //mobileFPSGame.GameOverPanel.GetComponent<GameOverPanel>().playerBehaviour = GetComponent<PlayerBehaviour>();
                mobileFPSGame.GameOverPanel.GetComponent<GameOverPanel>().position = rank;
                mobileFPSGame.GameOverPanel.SetActive(true);
            }
        }


        public void ExitFunctionCalled()
        {
            if (photonView.IsMine)
            {
                PhotonNetwork.LeaveLobby();
                PhotonNetwork.LeaveRoom();
                PhotonNetwork.Disconnect();
                //PhotonNetwork.LoadLevel("LobbyScene");
            }
        }


        public void setSkinnedMeshRenderer(bool setRender)
        {
            skinnedMeshRenderer.enabled = setRender;
        }

        public PunController getPunController()
        {
            return punController;
        }

        public void EnableCollider(bool enable)
        {
            GetComponent<TPSShooter.PlayerBehaviour>()._characterController.enabled = enable;
        }

        [ContextMenu("Kill Player")]
        public void testDie()
        {
            Die();
        }

        /*[PunRPC]
        public void NotifyFiring()
        {
            if (!photonView.IsMine)
            {
                // Start the coroutine to check for firing if it hasn't started yet
                photonView.RPC(nameof(ShowEnemyOnMinimap), RpcTarget.All);
            }
        }*/

        
    }



}