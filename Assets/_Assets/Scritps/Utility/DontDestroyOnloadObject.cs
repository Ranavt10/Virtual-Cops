﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnloadObject : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
