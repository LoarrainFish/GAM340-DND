using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeTextTransparent : MonoBehaviour {


    public static Animator Anim;

    private void Start()
    {
        Anim = GetComponent<Animator>();
    }

    public static void Transparent()
    {
        Anim.SetBool("TextFade", true);
    }
    public static void TransparentEnd()
    {
        Anim.SetBool("TextFade", false);
    }
}
