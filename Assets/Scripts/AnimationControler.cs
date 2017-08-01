using UnityEngine;

using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using UnityEngine.Audio;
//using System.Runtime;

public class AnimationControler : MonoBehaviour
{
    public static AnimationControler instance;

    private Animator shield_anim, sword_anim, potion_anim, firebol_anim;
    public AnimationClip shine_shield;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    //animations
    public void PlaySword()
    {
        sword_anim.SetTrigger("attack");
    }
   
    public void PlayShield()
    {
        shield_anim.SetTrigger("defense");
    }
    public void PlayFirebol()
    {
        firebol_anim.SetTrigger("spell");
    }
    public void PlayPotion()
    {
        potion_anim.SetTrigger("heal");
    }

}