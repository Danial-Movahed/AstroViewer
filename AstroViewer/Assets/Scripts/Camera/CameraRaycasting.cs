using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CameraRaycasting : MonoBehaviour
{
    public float lockTime = 1.5f; 
    private bool locking = false;
    private float timestamp = 0.1f;
    private RaycastHit[] hit;
    public GameObject CamCanvas, DescCanvas;
    public Image progress;
    private bool isUiOpen=false;
    private GameObject starHitLoading;
    public GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    public EventSystem m_EventSystem;
    void Update()
    {
        if(!isUiOpen)
        {
            hit = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), Mathf.Infinity);
            foreach(RaycastHit h in hit)
            {
                if(h.collider.gameObject.tag == "Constellations")
                {
                    h.collider.gameObject.GetComponent<ConstellationAnimation>().ShowAnimation();
                }
                else if (!locking)
                {
                    starHitLoading = h.collider.gameObject;
                    locking = true;
                    timestamp = Time.time + lockTime;
                }
            }
            if(!Array.Exists(hit, x => x.collider.gameObject.Equals(starHitLoading)))
            {
                locking = false;
                starHitLoading = null;
            }
            if (locking && Time.time >= timestamp)
            {
                isUiOpen=true;
                locking = false;
                starHitLoading.GetComponent<DescUILoader>().Load(this.gameObject);
            }
            progress.fillAmount = Convert.ToInt32(locking) * (Time.time - timestamp + lockTime) / lockTime;
        }
        else
        {
            m_PointerEventData = new PointerEventData(m_EventSystem);
            m_PointerEventData.position = new Vector2(Screen.width/2,Screen.height/2);
            List<RaycastResult> results = new List<RaycastResult>();
            m_Raycaster.Raycast(m_PointerEventData, results);
            string rayCol;
            try
            {
                rayCol = results[0].gameObject.name;
                Debug.Log(rayCol);
            }
            catch
            {
                rayCol = null;
            }
            if( rayCol == "CloseBG")
            {
                if (!locking)
                {
                    locking = true;
                    timestamp = Time.time + lockTime;
                }
            }
            else
                locking = false;
            if (locking && Time.time >= timestamp)
            {
                DescCanvas.transform.GetChild(0).gameObject.SetActive(false);
                isUiOpen = false;
                locking = false;
            }
            progress.fillAmount = Convert.ToInt32(locking) * (Time.time - timestamp + lockTime) / lockTime;
        }
    }
}
