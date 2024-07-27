﻿using UnityEngine;
using System.Collections;

public class Torpedo : BaseBullet
{
    public override void Deactive()
    {
        base.Deactive();

        PoolingController.Instance.poolTorpedo.Store(this);
    }

    protected override void SpawnHitEffect()
    {
        EffectController.Instance.SpawnParticleEffect(EffectObjectName.BulletImpactExplodeMedium, transform.position);
        CameraFollow.Instance.AddShake(0.15f, 0.35f);
        SoundManager.Instance.PlaySfx(StaticValue.SOUND_SFX_EXPLOSIVE);
    }
}
