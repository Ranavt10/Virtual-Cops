using UnityEngine;

using LightDev;
using LightDev.UI;
using LightDev.Core;


namespace TPSShooter.UI
{
  public class EnemyHeadshot : CanvasElement
  {
    [Header("References")]
    public Base info;

    public override void Subscribe()
    {
      
      
    }

    public override void Unsubscribe()
    {
      
      
    }

    protected override void OnStartShowing()
    {
      info.SetFade(0);
    }

   
  }
}
