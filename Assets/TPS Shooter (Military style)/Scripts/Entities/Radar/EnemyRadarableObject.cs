using UnityEngine;
using TPSShooter.UI;

using LightDev;

namespace TPSShooter
{
  [RequireComponent(typeof(PlayerBehaviour))]
  public class EnemyRadarableObject : RadarableObject
  {
    private PlayerBehaviour enemy;

    private void Start()
    {
      enemy = GetComponent<PlayerBehaviour>();

      Events.EnemyKilled += OnEnemyKilled;
    }

    private void OnDestroy()
    {
      Events.EnemyKilled -= OnEnemyKilled;
    }

    private void OnEnemyKilled(PlayerBehaviour enemy)
    {
      if(this.enemy == enemy)
      {
        DestroyRadarableObject();
      }
    }
  }
}
