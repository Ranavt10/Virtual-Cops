using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMapManager : MonoBehaviour
{
    private void OnEnable()
    {
        MobileFPSGameManager.instance.uiHandler1?.Invoke();

    }

    private void OnDisable()
    {
        if (MobileFPSGameManager.instance)
        {
            /*if (MobileFPSGameManager.instance.playerCanvas)*/
            {
                MobileFPSGameManager.instance.uiHandler?.Invoke();
            }

        }


    }
}
