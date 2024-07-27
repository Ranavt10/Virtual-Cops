using System.Collections;
using UnityEngine;

using LightDev;

namespace TPSShooter
{
    public partial class PlayerBehaviour
    {
        public bool IsRunning { get; private set; }
        public bool IsCrouching { get; private set; }
        public bool IsJumping { get; private set; }
        public bool IsGrounded { get; private set; }

        private float _characterControllerInitialHeight;
        private Vector3 _characterControllerInitialCenter;

        private void InitializeCrouch()
        {
            _characterControllerInitialHeight = _characterController.height;
            _characterControllerInitialCenter = _characterController.center;
        }

        private void OnJumpRequested()
        {
            if (!IsAlive) return;


            if (IsCrouching)
                Stand();
            else
            {
                if (IsAiming)
                    DeactivateAiming();
                CharacterControllerJump();
            }
        }

        private void OnCrouchRequested()
        {
            if (!IsAlive) return;


            if (IsCrouching)
                Stand();
            else
                Crouch();
        }

        private void UpdateGroundCheck()
        {
            if (IsAlive == false) return;


            IsGrounded = /*IsGroundedReturn();*/CheckGrounded();
            //Debug.Log("Is Grounded is:" + IsGrounded);
            _animator.SetBool(animationsParameters.groundedBool, IsGrounded);

            _animator.SetBool(animationsParameters.jumpBool, IsJumping);
        }

        private float _forward;
        private float _strafe;
        private readonly float _movementLerpSpeed = 15f;

        private void UpdateWalk()
        {
            if (IsAlive == false) return;


            _forward = Mathf.MoveTowards(_forward, InputController.VerticalMovement, _movementLerpSpeed * Time.deltaTime);
            _strafe = Mathf.MoveTowards(_strafe, InputController.HorizontalMovement, _movementLerpSpeed * Time.deltaTime);

            _animator.SetFloat(animationsParameters.verticalMovementFloat, _forward);
            _animator.SetFloat(animationsParameters.horizontalMovementFloat, _strafe);
        }

        private void UpdateRun()
        {
            if (IsAlive == false) return;


            if (InputController.IsRun &&
                ((!IsUnarmedMode && _forward > 0.3f) || (IsUnarmedMode)) &&
                !IsReloading &&
                !IsThrowingGrenade &&
                !IsFire &&
                !IsJumping &&
                !IsCrouching &&
                !IsAiming
            )
                IsRunning = true;
            else
                IsRunning = false;

            _animator.SetBool(animationsParameters.runBool, IsRunning);
        }

        private void UpdateMovementSpeed()
        {
            if (IsAlive == false) return;


            Vector3 movement = new Vector3(_strafe, 0, _forward);
            movement.Normalize();

            if (IsGrounded)
            {
                if (movementSettings.ApplyRootMotion)
                {
                    movement = Vector3.zero;
                }
                else
                {
                    if (IsCrouching)
                    {
                        movement.x *= movementSettings.CrouchStrafeSpeed;
                        movement.z *= movementSettings.CrouchForwardSpeed;
                    }
                    else
                    {
                        movement.z *= IsRunning ? movementSettings.SprintSpeed : movementSettings.ForwardSpeed;
                        movement.x *= movementSettings.StrafeSpeed;
                    }
                }
            }
            else
            {
                movement *= movementSettings.AirSpeed;
            }

            movement = transform.TransformDirection(movement);
            _characterController.Move(movement * Time.deltaTime);
        }

        private bool _resetGravity;
        private float _gravity;

        private float _gravityModifier = 9.81f;
        private float _baseGravity = 50.0f;
        private float _resetGravityValue = 1.2f;

        private void UpdateGravity()
        {
            if (IsAlive == false) return;


            if (!IsGrounded)
            {
                if (!_resetGravity)
                {
                    _gravity = _resetGravityValue;
                    _resetGravity = true;
                }
                _gravity += Time.deltaTime * _gravityModifier;
            }
            else
            {
                _gravity = _baseGravity;
                _resetGravity = false;
            }

            Vector3 gravityVector = new Vector3();

            if (_jumpingTriggered)
                gravityVector.y = movementSettings.JumpSpeed;
            else
                gravityVector.y -= _gravity;

            _characterController.Move(gravityVector * Time.deltaTime);
        }

        private float groundCheckDistance = 0.3f;  // Distance to check for ground
        private float groundNormalThreshold = 0.3f;

        /*private bool CheckGrounded()
        {
            *//*RaycastHit hit;
            Vector3 start = transform.position + transform.up;
            Vector3 dir = Vector3.down;
            float radius = _characterController.radius;
            if (Physics.SphereCast(start, radius, dir, out hit, _characterController.height * 0.6f))
                return true;

            return false;*//*

            RaycastHit hit;
            Vector3 start = transform.position + Vector3.up * 0.1f; // Start slightly above the player's position
            Vector3 dir = Vector3.down;
            float distance = 0.2f; // Small distance to check directly beneath the player
            //LayerMask groundLayer = LayerMask.GetMask("Ground"); // Adjust this to your ground layer

            if (Physics.Raycast(start, dir, out hit, distance *//*groundLayer*//*))
            {
                // Check the normal of the hit surface to ensure it's relatively horizontal
                if (Vector3.Angle(hit.normal, Vector3.up) < 10f)
                {
                    return true;
                }
            }

            return false;
        }*/

        private bool CheckGrounded()
        {
            /*Vector3[] raycastOrigins = CalculateRaycastOrigins(); // Calculate multiple raycast origins around the player's base
            float distance = 0.2f; // Small distance to check directly beneath the player
            //LayerMask groundLayer = LayerMask.GetMask("Ground"); // Adjust this to your ground layer

            foreach (Vector3 origin in raycastOrigins)
            {
                RaycastHit hit;
                Vector3 dir = Vector3.down;

                if (Physics.Raycast(origin, dir, out hit, distance))//, groundLayer))
                {
                    // Check if the hit surface is relatively horizontal
                    if (Vector3.Angle(hit.normal, Vector3.up) <= 90f)
                    {
                        return true;
                    }
                }
            }

            return false;*/

            // Perform a raycast downwards from the player's position
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance))
            {
                // Check if the normal of the surface we hit is close to vertical
                if (Vector3.Dot(hit.normal, Vector3.up) > 1 - groundNormalThreshold)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /*private bool isGrounded1;
        private float sphereCastRadius = 0.3f;

        void CheckGroundStatus()
        {
            isGrounded1 = false;
            RaycastHit hit;

            // Perform a SphereCast downwards from the player's position
            if (Physics.SphereCast(transform.position, sphereCastRadius, Vector3.down, out hit, groundCheckDistance))
            {
                // Check if the normal of the surface we hit is close to vertical
                if (Vector3.Dot(hit.normal, Vector3.up) > 1 - groundNormalThreshold)
                {
                    isGrounded1 = true;
                }
            }
        }

        public bool IsGroundedReturn()
        {
            return isGrounded1;
        }*/

        private Vector3[] CalculateRaycastOrigins()
        {
            // Define multiple raycast origins around the player's base
            Vector3[] origins = new Vector3[4];

            // You may adjust these values based on the size and shape of your player character
            float offsetX = 0.25f;
            float offsetZ = 0.25f;

            // Calculate raycast origins
            origins[0] = transform.position + Vector3.up * 0.1f; // Center
            origins[1] = transform.position + Vector3.up * 0.1f + Vector3.right * offsetX; // Right
            origins[2] = transform.position + Vector3.up * 0.1f - Vector3.right * offsetX; // Left
            origins[3] = transform.position + Vector3.up * 0.1f + Vector3.forward * offsetZ; // Forward

            return origins;
        }

        private bool _jumpingTriggered;

        // Makes the character jump
        private void CharacterControllerJump()
        {
            if (_jumpingTriggered)
                return;

            if (IsGrounded)
            {
                sounds.PlaySound(sounds.JumpSound);

                _jumpingTriggered = true;
                StartCoroutine(StopJump());
            }
        }

        // Stops us from jumping
        private IEnumerator StopJump()
        {
            yield return new WaitForSeconds(movementSettings.JumpTime);
            _jumpingTriggered = false;
        }

        // Animation event
        private void StartJumping()
        {
            //IsJumping = true;
        }

        // Animation event
        private void FinishedJumping()
        {
            IsJumping = false;
        }

        // Animation event
        private void LandSound()
        {
            sounds.PlaySound(sounds.LandSound);
        }

        private void Crouch()
        {
            IsCrouching = true;
            _animator.SetBool(animationsParameters.crouchBool, IsCrouching);

            _characterController.center = crouchSettings.CharacterCenterCrouching;
            _characterController.height = crouchSettings.CharacterHeightCrouching;

            Events.PlayerCrouch.Call();
        }

        private void Stand()
        {
            IsCrouching = false;
            _animator.SetBool(animationsParameters.crouchBool, IsCrouching);

            _characterController.center = _characterControllerInitialCenter;
            _characterController.height = _characterControllerInitialHeight;

            Events.PlayerStand.Call();
        }
    }
}
