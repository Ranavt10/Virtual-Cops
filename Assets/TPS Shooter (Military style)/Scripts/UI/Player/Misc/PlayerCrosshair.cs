using UnityEngine;
using UnityEngine.UI;

using LightDev;
using LightDev.UI;

namespace TPSShooter.UI
{
  public class PlayerCrosshair : CanvasElement
  {
    [Header("References")]
    public Image peacefulCrosshair;
    public Image enemyCrosshair;

    [Header("Preferences")]
    public bool showCrosshairWhileRunning = true;

    private PlayerBehaviour _player;

    private void Start()
    {
      
      _player = PlayerBehaviour.GetInstance();
    }

    public override void Subscribe()
    {
     
      Events.PlayerDied += Hide;
    }

    public override void Unsubscribe()
    {
     
      Events.PlayerDied -= Hide;
    }

    private void UpdateAim()
    {
      if ((!showCrosshairWhileRunning && _player.IsRunning) ||
        _player.IsUnarmedMode ||
        _player.IsThrowingGrenade ||
        (_player.IsAiming && _player.CurrentWeaponBehaviour.ScopeSettings.IsFPS)
      )
      {
        enemyCrosshair.enabled = false;
        peacefulCrosshair.enabled = false;
      }
      else
      {
        if (_player.FireHitObject != null
          && _player.FireHitObject.GetComponentInParent<PlayerBehaviour>() )
        {
          enemyCrosshair.enabled = true;
          peacefulCrosshair.enabled = false;
        }
        else
        {
          enemyCrosshair.enabled = false;
          peacefulCrosshair.enabled = true;
        }
      }
    }

    private void Update()
    {
      if (_player.IsAlive)
        UpdateAim();
    }
  }
}
