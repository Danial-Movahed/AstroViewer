using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ConstellationAnimation : MonoBehaviour
{
    private bool isShown = false;
    public int timeout = 0;
    IEnumerator checkTimeout()
    {
        Debug.Log(timeout);
        if (timeout <= 0)
        {
            isShown = false;
            CloseAnimation();
            yield return null;
        }
        else
        {
            timeout -= 1;
            yield return new WaitForSeconds(1);
            StartCoroutine(checkTimeout());
        }
    }
    void CloseAnimation()
    {
        Debug.Log("Closing!");
        for (int i = 0; i < transform.childCount; i++)
        {
            try
            {
                transform.GetChild(i).GetComponent<Animator>().Play("LineAnimationClose");
            }
            catch { }
        }
    }
    public void ShowAnimation()
    {
        if(!isShown)
        {
            isShown = true;
            for (int i = 0; i < transform.childCount; i++)
            {
                try
                {
                    transform.GetChild(i).GetComponent<Animator>().Play("LineAnimationOpen");
                }
                catch { }
            }
            timeout = 1;
            StartCoroutine(checkTimeout());
        }
        else
        {
            timeout = 1;
        }
    }
}