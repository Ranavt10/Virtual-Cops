using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapManager : Singleton<MiniMapManager>
{
    public MiniMapController mapController;
    public MiniMapController bigMapController;

    [SerializeField]
    private LayerMask layerMask;
    // Start is called before the first frame update
    void Start()
    {
        mapController.minimapLayers = layerMask;
        mapController.mapCamera.cullingMask = layerMask;
    }
}
