using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLocationRise : MonoBehaviour {

    public static Animator Anim;

    private void Start()
    {
        Anim = GetComponent<Animator>();
    }

    public static void AdventureRise()
    {
        Anim.SetBool("AreaRise", true);
    }
    public static void AdventureRiseEnd()
    {
        Anim.SetBool("ArseRise", false);
    }

}
