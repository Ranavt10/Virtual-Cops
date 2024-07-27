using System.Collections;
using UnityEngine;

using LightDev;
using LightDev.Core;



namespace TPSShooter
{
    [ExecuteInEditMode]
    public partial class TPSCamera : Base
    {
        [Header("- Target -")]
        public Transform target;

        [Header("- Camera Rig Components -")]
        public Transform pivot;
        public Transform collidedPosition;
        public Transform cameraContainer;
        public Transform cameraTransform;

        [Header(" - Settings -")]
        public PlayerCameraSettings playerCameraSettings;


        private Camera _camera;
        private Vector3 cachedPivotLocalPos;

        private CameraPlayerState playerState;
        private CameraPlayerAimingState playerAimingState;

        private NoMovementState noMovementState;
        private CameraState currentState;

        private const string CrouchSequenceID = "crouch";
        private const string FireShakeSequenceID = "fireSh";

        #region MonoBehaviour

        private void OnValidate()
        {

            playerCameraSettings.normalFieldOfView = Mathf.Clamp(playerCameraSettings.normalFieldOfView, 1, 179);
            playerCameraSettings.standMaxAngle = Mathf.Clamp(playerCameraSettings.standMaxAngle, -90, 90);
            playerCameraSettings.standMinAngle = Mathf.Clamp(playerCameraSettings.standMinAngle, -90, playerCameraSettings.standMaxAngle);
            playerCameraSettings.crouchMaxAngle = Mathf.Clamp(playerCameraSettings.crouchMaxAngle, -90, 90);
            playerCameraSettings.crouchMinAngle = Mathf.Clamp(playerCameraSettings.crouchMinAngle, -90, playerCameraSettings.crouchMaxAngle);




        }

        public Vector3 containerCameraVal;

        private void Awake()
        {
            containerCameraVal = cameraContainer.localPosition;

            Subscribe();

            _camera = cameraTransform.GetComponent<Camera>();

            cachedPivotLocalPos = pivot.localPosition;

            playerState = new CameraPlayerState(this, cameraContainer.localPosition);
            playerAimingState = new CameraPlayerAimingState(this);

            noMovementState = new NoMovementState(this);
            ChangeState(playerState);


        }

        private void OnDestroy()
        {

            Unsubscribe();


        }

        private void LateUpdate()
        {

            if (!Application.isPlaying) // moves camera rig, while game is not playing
            {
                //target = GameObject.FindObjectOfType<PlayerBehaviour>().transform;

                if (target != null)
                {
                    transform.position = target.position;
                    transform.rotation = target.rotation;
                }
            }
            else
            {
                currentState.OnUpdate();
            }

        }

        #endregion

        private void Subscribe()
        {

            Events.PlayerAimActivated += OnPlayerAimActivated;
            Events.PlayerAimDeactivated += OnPlayerAimDeactivated;

            Events.PlayerCrouch += OnPlayerCrouch;

            Events.PlayerFire += OnPlayerFire;

            Events.PlayerDied += OnPlayerDied;


        }

        private void Unsubscribe()
        {

            Events.PlayerAimActivated -= OnPlayerAimActivated;
            Events.PlayerAimDeactivated -= OnPlayerAimDeactivated;
            Events.PlayerCrouch -= OnPlayerCrouch;
            Events.PlayerFire -= OnPlayerFire;
            Events.PlayerDied -= OnPlayerDied;


        }

        private void OnPlayerAimActivated()
        {
            ChangeState(playerAimingState);
        }

        private void OnPlayerAimDeactivated()
        {
            ChangeState(playerState);
        }



        private void OnPlayerCrouch()
        {
            Vector3 endPoint = cachedPivotLocalPos + playerCameraSettings.crouchPivotPositionOffset;

        }



        private void OnPlayerFire()
        {

            var weapon = PlayerBehaviour.GetInstance().CurrentWeaponBehaviour;
            FireCameraShake(weapon);
            FireCameraDeviation(weapon);



        }

        private void OnPlayerDied()
        {
            ChangeState(noMovementState);
        }

        private void FireCameraShake(PlayerWeapon weapon)
        {


            float duration = weapon.ShootFrequency / 2;
            float shakeForce = weapon.CameraShakeForce;
            float strength = Random.Range(0, 100) > 50 ? shakeForce : -shakeForce;



        }

        private void FireCameraDeviation(PlayerWeapon weapon)
        {

            float rotationX = Random.Range(weapon.MinDeviationAlongY, weapon.MaxDeviationAlongY);
            float rotationY = Random.Range(-weapon.DeviationAlongX, weapon.DeviationAlongX);
            if (currentState == playerState)
            {
                playerState.RotateCameraRig(rotationY);
                playerState.RotatePlayerPivot(rotationX);
                playerAimingState.UpdatePlayerRotation();
            }
            else if (currentState == playerAimingState)
            {
                playerAimingState.RotateCameraRig(rotationY);
                playerAimingState.RotatePlayerPivot(rotationX);
                playerAimingState.UpdatePlayerRotation();
            }

        }

        private void ChangeState(CameraState state)
        {

            if (currentState == noMovementState) return;

            currentState?.OnExit();
            currentState = state;
            currentState.OnEnter();


        }
    }
}
