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
            if(transform.GetChild(i).tag == "ConstelLine")
            {
                transform.GetChild(i).GetComponent<Animator>().Play("LineAnimationClose");
                continue;
            }
            if(transform.GetChild(i).tag == "ConstelName")
            {
                transform.GetChild(i).GetComponent<Animator>().Play("ConstelTextHide");
                continue;
            }
        }
    }
    public void ShowAnimation()
    {
        if(!isShown)
        {
            isShown = true;
            for (int i = 0; i < transform.childCount; i++)
            {
                if(transform.GetChild(i).tag == "ConstelLine")
                {
                    transform.GetChild(i).GetComponent<Animator>().Play("LineAnimationOpen");
                    continue;
                }
                if(transform.GetChild(i).tag == "ConstelName")
                {
                    transform.GetChild(i).GetComponent<Animator>().Play("ConstelTextShow");
                    continue;
                }
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