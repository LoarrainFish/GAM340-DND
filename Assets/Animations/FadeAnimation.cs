using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAnimation : MonoBehaviour {

    public static Animator Anim;

    private void Start()
    {
        Anim = GetComponent<Animator>();
    }

    public static void TriggerAnimation()
    {
        Anim.SetBool("FadeToBlack", true);
    }

    public static void EndTriggerAnimation()
    {
        Anim.SetBool("FadeToBlack", false);
    }

}
