﻿using UnityEngine;
using System.Collections;

public class SupportBomb : BaseSupportItem
{

    public override void Init()
    {
        base.Init();

        groupFree.SetActive(true);
        groupPrice.SetActive(false);
        priceUse = 0;
    }

    protected override void Consume()
    {
        if (GameDataNEW.playerResources.gem >= priceUse)
        {
            GameDataNEW.playerResources.ConsumeGem(priceUse);

            base.Consume();

            if (countUsed >= 2)
            {
                Active(false);
            }
            else if (countUsed >= 1)
            {
                groupFree.SetActive(false);
                groupPrice.SetActive(true);
                priceUse = StaticValue.COST_SUPPORT_ITEM_BOMB;
                textPrice.text = priceUse.ToString();
                textPrice.color = GameDataNEW.playerResources.gem >= priceUse ? Color.yellow : StaticValue.colorNotEnoughMoney;
            }

            EventDispatcher.Instance.PostEvent(EventID.UseSupportItemBomb);

            //FirebaseAnalyticsHelper.LogEvent("N_UseSurvivalSupportItem", "Bomb");
        }
    }
}
