using LightDev;
using LightDev.UI;

namespace TPSShooter.UI
{
  public class PlayerWeaponMobileInput : CanvasElement
  {
    public override void Subscribe()
    {
      base.Subscribe();

     
     
      Events.PlayerShowWeapon += NeedShow;
      Events.PlayerDropWeapon += OnPlayerDropWeapon;
      Events.PlayerHideWeapon += Hide;
     
      
    }

    public override void Unsubscribe()
    {
      base.Unsubscribe();

     
     
      Events.PlayerShowWeapon -= NeedShow;
      Events.PlayerDropWeapon -= OnPlayerDropWeapon;
      Events.PlayerHideWeapon -= Hide;
     
      
    }

    protected virtual void NeedShow()
    {
            

            Show();
    }

    protected virtual void OnPlayerDropWeapon(PlayerWeapon weapon)
    {
      Hide();
    }

    public void OnFireStay() { Events.FireRequested.Call(); }
    public void OnReloadClick() { Events.ReloadRequested.Call(); }
    public void OnAimActivateClick() { Events.AimActivateRequested.Call(); }
  }
}
