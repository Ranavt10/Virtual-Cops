using UnityEngine;
using UnityEngine.UI;

using LightDev;
using LightDev.UI;

namespace TPSShooter.UI
{
  public class PlayerHP : CanvasElement
  {
    [Header("References")]
    public Image healthBar;
    public Image otherhealthBar;

    public override void Subscribe()
    {
            if (photonView.IsMine)
            {
                Events.PlayerDied += Hide;
                Events.PlayerChangedHP += OnPlayerChangedHP;
                
            }
            
    }

    public override void Unsubscribe()
    {
            if (photonView.IsMine)
            {
                Events.PlayerDied -= Hide;
                Events.PlayerChangedHP -= OnPlayerChangedHP;
                
            }
            
    }

    private void OnPlayerChangedHP()
    {
      var player = PlayerBehaviour.GetInstance();
      healthBar.fillAmount = player.GetCurrentHP() / player.GetMaxHP();
      otherhealthBar.fillAmount = player.GetCurrentHP() / player.GetMaxHP();
    }
    }
}