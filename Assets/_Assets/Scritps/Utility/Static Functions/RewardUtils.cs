using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class RewardUtils
{
    public static void Receive(List<RewardData> rewards)
    {
        for (int i = 0; i < rewards.Count; i++)
        {
            RewardData reward = rewards[i];

            switch (reward.type)
            {
                case RewardType.Coin:
                    GameDataNEW.playerResources.ReceiveCoin(reward.value);
                    break;

                case RewardType.Gem:
                    GameDataNEW.playerResources.ReceiveGem(reward.value);
                    break;

                case RewardType.Stamina:
                    GameDataNEW.playerResources.ReceiveStamina(reward.value);
                    break;

                case RewardType.Medal:
                    GameDataNEW.playerResources.ReceiveMedal(reward.value);
                    break;

                case RewardType.TournamentTicket:
                    GameDataNEW.playerResources.ReceiveTournamentTicket(reward.value);
                    break;

                case RewardType.Exp:
                    GameDataNEW.playerProfile.ReceiveExp(reward.value);
                    break;

                case RewardType.BoosterHp:
                    GameDataNEW.playerBoosters.Receive(BoosterType.Hp, reward.value);
                    break;

                case RewardType.BoosterDamage:
                    GameDataNEW.playerBoosters.Receive(BoosterType.Damage, reward.value);
                    break;

                case RewardType.BoosterCoinMagnet:
                    GameDataNEW.playerBoosters.Receive(BoosterType.CoinMagnet, reward.value);
                    break;

                case RewardType.BoosterSpeed:
                    GameDataNEW.playerBoosters.Receive(BoosterType.Speed, reward.value);
                    break;

                case RewardType.BoosterCritical:
                    GameDataNEW.playerBoosters.Receive(BoosterType.Critical, reward.value);
                    break;

                case RewardType.GunM4:
                    if (GameDataNEW.playerGuns.ContainsKey(StaticValue.GUN_ID_M4))
                    {
                        reward.type = RewardType.Gem;
                        reward.value = GameDataNEW.gunValueGem[StaticValue.GUN_ID_M4];
                        GameDataNEW.playerResources.ReceiveGem(reward.value);
                    }
                    else
                    {
                        GameDataNEW.playerGuns.ReceiveNewGun(StaticValue.GUN_ID_M4);
                    }
                    break;

                case RewardType.GunSpread:
                    if (GameDataNEW.playerGuns.ContainsKey(StaticValue.GUN_ID_SPREAD))
                    {
                        reward.type = RewardType.Gem;
                        reward.value = GameDataNEW.gunValueGem[StaticValue.GUN_ID_SPREAD];
                        GameDataNEW.playerResources.ReceiveGem(reward.value);
                    }
                    else
                    {
                        GameDataNEW.playerGuns.ReceiveNewGun(StaticValue.GUN_ID_SPREAD);
                    }
                    break;

                case RewardType.GunScarH:
                    if (GameDataNEW.playerGuns.ContainsKey(StaticValue.GUN_ID_SCAR_H))
                    {
                        reward.type = RewardType.Gem;
                        reward.value = GameDataNEW.gunValueGem[StaticValue.GUN_ID_SCAR_H];
                        GameDataNEW.playerResources.ReceiveGem(reward.value);
                    }
                    else
                    {
                        GameDataNEW.playerGuns.ReceiveNewGun(StaticValue.GUN_ID_SCAR_H);
                    }
                    break;

                case RewardType.GunBullpup:
                    if (GameDataNEW.playerGuns.ContainsKey(StaticValue.GUN_ID_BULLPUP))
                    {
                        reward.type = RewardType.Gem;
                        reward.value = GameDataNEW.gunValueGem[StaticValue.GUN_ID_BULLPUP];
                        GameDataNEW.playerResources.ReceiveGem(reward.value);
                    }
                    else
                    {
                        GameDataNEW.playerGuns.ReceiveNewGun(StaticValue.GUN_ID_BULLPUP);
                    }
                    break;

                case RewardType.GunKamePower:
                    if (GameDataNEW.playerGuns.ContainsKey(StaticValue.GUN_ID_KAME_POWER))
                    {
                        reward.type = RewardType.Gem;
                        reward.value = GameDataNEW.gunValueGem[StaticValue.GUN_ID_KAME_POWER];
                        GameDataNEW.playerResources.ReceiveGem(reward.value);
                    }
                    else
                    {
                        GameDataNEW.playerGuns.ReceiveNewGun(StaticValue.GUN_ID_KAME_POWER);
                    }
                    break;

                case RewardType.GunSniperRifle:
                    if (GameDataNEW.playerGuns.ContainsKey(StaticValue.GUN_ID_SNIPER_RIFLE))
                    {
                        reward.type = RewardType.Gem;
                        reward.value = GameDataNEW.gunValueGem[StaticValue.GUN_ID_SNIPER_RIFLE];
                        GameDataNEW.playerResources.ReceiveGem(reward.value);
                    }
                    else
                    {
                        GameDataNEW.playerGuns.ReceiveNewGun(StaticValue.GUN_ID_SNIPER_RIFLE);
                    }
                    break;

                case RewardType.GunTeslaMini:
                    if (GameDataNEW.playerGuns.ContainsKey(StaticValue.GUN_ID_TESLA_MINI))
                    {
                        reward.type = RewardType.Gem;
                        reward.value = GameDataNEW.gunValueGem[StaticValue.GUN_ID_TESLA_MINI];
                        GameDataNEW.playerResources.ReceiveGem(reward.value);
                    }
                    else
                    {
                        GameDataNEW.playerGuns.ReceiveNewGun(StaticValue.GUN_ID_TESLA_MINI);
                    }
                    break;

                case RewardType.GunLaser:
                    if (GameDataNEW.playerGuns.ContainsKey(StaticValue.GUN_ID_LASER))
                    {
                        reward.type = RewardType.Gem;
                        reward.value = GameDataNEW.gunValueGem[StaticValue.GUN_ID_LASER];
                        GameDataNEW.playerResources.ReceiveGem(reward.value);
                    }
                    else
                    {
                        GameDataNEW.playerGuns.ReceiveNewGun(StaticValue.GUN_ID_LASER);
                    }
                    break;

                case RewardType.GunFlame:
                    if (GameDataNEW.playerGuns.ContainsKey(StaticValue.GUN_ID_FLAME))
                    {
                        reward.type = RewardType.Gem;
                        reward.value = GameDataNEW.gunValueGem[StaticValue.GUN_ID_FLAME];
                        GameDataNEW.playerResources.ReceiveGem(reward.value);
                    }
                    else
                    {
                        GameDataNEW.playerGuns.ReceiveNewGun(StaticValue.GUN_ID_FLAME);
                    }
                    break;

                case RewardType.MeleeWeaponPan:
                    GameDataNEW.playerMeleeWeapons.ReceiveNewMeleeWeapon(StaticValue.MELEE_WEAPON_ID_PAN);
                    break;

                case RewardType.MeleeWeaponGuitar:
                    GameDataNEW.playerMeleeWeapons.ReceiveNewMeleeWeapon(StaticValue.MELEE_WEAPON_ID_GUITAR);
                    break;

                case RewardType.GrenadeF1:
                    GameDataNEW.playerGrenades.Receive(StaticValue.GRENADE_ID_F1, reward.value);
                    break;

                case RewardType.GrenadeTet:
                    GameDataNEW.playerGrenades.Receive(StaticValue.GRENADE_ID_TET_HOLIDAY, reward.value);
                    break;
            }
        }
    }
}
