using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBob : MonoBehaviour {

    
    public static Animator Anim;

    private void Start()
    {
        Anim = GetComponent<Animator>();
    }

    public static void TextBob1()
    {
        //Anim.SetBool("TextBob", true);
    }
    public static void TextBob1Text()
    {
        Anim.SetBool("TextBob", false);
    }
}
