using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

using LightDev;

namespace TPSShooter
{
  // This class controlls state of the game.
  // It can stop/resume/finish the game 
  //        (when the game is stopped/resumed/finished this class notifies other GameObjects that subscribes to events).
  // It can also download Menu scene and Play scene.
  public class GameManager : MonoBehaviour
  {
    public static bool IsGamePaused { get; private set; }
    public static bool IsGameFinished { get; private set; }

    private void Awake()
    {
      IsGamePaused = false;
      IsGameFinished = false;

     
      Events.PlayerDied += OnPlayerDied;
    }

    private void OnDestroy()
    {
      
      Events.PlayerDied -= OnPlayerDied;
    }


    private void OnPlayerDied()
    {
      if (IsGameFinished) return;

     
    }

   

   


    

    

   
  }
}
