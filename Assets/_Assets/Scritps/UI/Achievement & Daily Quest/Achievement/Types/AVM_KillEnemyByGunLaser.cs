﻿using UnityEngine;
using System.Collections;

public class AVM_KillEnemyByGunLaser : BaseAchievement
{
    public override void Init()
    {
        base.Init();

        EventDispatcher.Instance.RegisterListener(EventID.UnitDie, (sender, param) =>
        {
            UnitDieData data = (UnitDieData)param;

            if (data.attackData == null || data.attackData.weaponId == -1)
                return;

            StaticGunData gun = GameDataNEW.staticGunData.GetData(data.attackData.weaponId);

            if (gun != null && gun.id == StaticValue.GUN_ID_LASER)
            {
                IncreaseProgress();
            }
        });
    }
}
