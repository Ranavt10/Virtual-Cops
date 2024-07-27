using UnityEngine;
using UnityEngine.UI;

namespace TPSShooter
{
  public class PlayerGrenade : AbstractGrenade
  {
    [Header("UI")]
    public Image fillImage;
    public Text infoText;

    protected override void AbstractImpact(Collider[] nearbyColliders)
    {
      foreach (Collider nearbyObject in nearbyColliders)
      {
                
                 if (nearbyObject.GetComponent<PlayerBehaviour>())
                {
                    if (photonView.IsMine)
                    {
                        nearbyObject.GetComponent<PlayerBehaviour>().OnGrenadeHit(this);
                    }
                }
                //else if (nearbyObject.GetComponent<EnemyBehaviour>())
                //{

                //    //jamil nearbyObject.GetComponent<EnemyBehaviour>().OnGrenadeHit(this);
                //change garnade code here.....

                //}
            }
    }

    private void Update()
    {
      UpdateInfoText();
      UpdateImageFill();
    }

    private void UpdateInfoText()
    {
      if(infoText == null) return;

      infoText.text = $"{Mathf.CeilToInt(currentDelay):00}s";
    }

    private void UpdateImageFill()
    {
      if(fillImage == null) return;

      fillImage.fillAmount = currentDelay / delay;
    }
  }
}
