using UnityEngine;

using LightDev;
using LightDev.UI;

namespace TPSShooter.UI.Menu
{
  public class Menu : CanvasElement
  {
    public override void Subscribe()
    {
      
      Events.RequestMenu += Show;
    }

    public override void Unsubscribe()
    {
      
      Events.RequestMenu -= Show;
    }

    public void OnPlay()
    {
      Events.MenuClickSound.Call();
      Events.RequestMenuWeapon.Call();
      Hide();
    }

    public void OnSettings()
    {
      Events.MenuClickSound.Call();
      Events.RequestMenuSettings.Call();
      Hide();
    }

   
  }
}
