using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    public MobileFPSGameManager mobileFPSGame;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    [ContextMenu("Re Arrange")]
    public void ReArrange()
    {
        //mobileFPSGame.RemoveAndRearrange(gameObject);
    }
}
