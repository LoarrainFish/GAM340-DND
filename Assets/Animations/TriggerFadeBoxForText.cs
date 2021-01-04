using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFadeBoxForText : MonoBehaviour {

    public static Animator Anim;

    private void Start()
    {
        Anim = GetComponent<Animator>();
    }

    public static void TriggerAnimation()
    {
        Anim.SetBool("FadeStatsIn", true);
    }

    public static void TriggerAnimation2()
    {
        Anim.SetBool("FadeStatsIn2", true);
    }

    public static void TriggerAnimation3()
    {
        Anim.SetBool("FadeStatsIn3", true);
    }

}
