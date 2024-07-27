using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationclose : MonoBehaviour
{
    public Animator winanimation;

    public void closeanim()
    {
        winanimation.SetBool("Win", false);
    }
}
