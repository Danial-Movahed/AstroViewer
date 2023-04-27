using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBtnAnimationController : MonoBehaviour
{
    private Animation btnAnimation;
    void Start()
    {
        btnAnimation = gameObject.GetComponent<Animation>();
    }
    public void PlayAnimation(string animName)
    {
        btnAnimation.Play(animName);
    }
}
