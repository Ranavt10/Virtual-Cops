using UnityEngine;

using LightDev;
using LightDev.UI;

namespace TPSShooter.UI
{
  public class PlayerMobileInput : CanvasElement
  {
    [Header("References")]
    public VirtualJoystick joystick;

       

    public override void Subscribe()
    {
      base.Subscribe();
            Show();
             


        }

        public override void Unsubscribe()
    {
      base.Unsubscribe();

      
      
    }

   

    protected virtual void Update()
    {
      InputController.HorizontalMovement = joystick.HorizontalValue;
      InputController.VerticalMovement = joystick.VerticalValue;
      InputController.IsRun = joystick.IsRun;
    }

    public void OnCrouchClick() { Events.CrouchRequested.Call(); }
    public void OnJumpClick() { Events.JumpRequested.Call(); }

    public void OnGrenadeThrowDown() { Events.GrenadeStartThrowRequest.Call(); }
    public void OnGrenadeThrowUp() { Events.GrenadeFinishThrowRequest.Call(); }
        private void Start()
        {
            OnWeaponChooseRequest();
        }
        public void OnWeaponChooseRequest() 
        {
           
            Events.WeaponChooseStartRequest.Call(); 
        }
  }
}
