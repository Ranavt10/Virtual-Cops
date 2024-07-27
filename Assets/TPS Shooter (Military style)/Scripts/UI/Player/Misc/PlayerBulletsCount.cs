using UnityEngine;
using UnityEngine.UI;

using LightDev;
using LightDev.UI;

namespace TPSShooter.UI
{
  public class PlayerBulletsCount : CanvasElement
  {
    [Header("References")]
    public Text bulletsCountText;

    public override void Subscribe()
    {
      
      Events.PlayerDied += Hide;
      
      Events.PlayerHideWeapon += Hide;
      Events.PlayerShowWeapon += Show;

      Events.PlayerDropWeapon += PlayerDropWeapon;
      
     

      Events.PlayerReloaded += UpdateWeaponCountText;
      Events.PlayerFire += UpdateWeaponCountText;
      Events.PlayerPickUpAmmo += UpdateWeaponCountText;
    }

    public override void Unsubscribe()
    {
      
      Events.PlayerDied -= Hide;
      
      Events.PlayerHideWeapon -= Hide;
      Events.PlayerShowWeapon -= Show;

      Events.PlayerDropWeapon -= PlayerDropWeapon;
      

     
      Events.PlayerReloaded -= UpdateWeaponCountText;
      Events.PlayerFire -= UpdateWeaponCountText;
      Events.PlayerPickUpAmmo -= UpdateWeaponCountText;
    }

    private void TryShow()
    {
      if (PlayerBehaviour.GetInstance() == null)
      {
        Hide();
        return;
      }
      
      if (PlayerBehaviour.GetInstance().CurrentWeaponBehaviour == null)
      {
        Hide();
        return;
      }

      Show();
    }

    protected override void OnStartShowing()
    {
      UpdateWeaponCountText();
    }

    private void PlayerDropWeapon(PlayerWeapon weapon)
    {
      Hide();
    }

    private void UpdateWeaponCountText()
    {
      var weapon = PlayerBehaviour.GetInstance().CurrentWeaponBehaviour;
      bulletsCountText.text = string.Format("{0}/{1}", weapon.BulletsInMag, weapon.BulletsAmount);
    }
  }
}
