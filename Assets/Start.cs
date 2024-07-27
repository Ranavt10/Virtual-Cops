using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System;


public class Start : MonoBehaviour
{
    public MapChooser MapChooserScript;

    void OnEnable()
    {
        Invoke("scrptonkro", 1);

    }
    public static MapType GetMapType(string stageId)
    {
        string s = stageId.Split('.').First();
        int mapId = int.Parse(s);

        return (MapType)mapId;
    }
    public void scrptonkro()
    {
        MapChooserScript.enabled = true;
    }
}
