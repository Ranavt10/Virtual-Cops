using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogoBanner : Singleton<LogoBanner>
{
    public GameObject LogoPanel;
    public GameObject BannerPanel;
    public Image showCaseAvatarImage;
    public Image showCaseBannerImage;
    public Button avatarPurchaseBtn;
    public Button bannerPurchaseBtn;

    public Image profileLogo;
    public Sprite[] profileLogos;
    // Start is called before the first frame update
    void Start()
    {
        
    }
}
